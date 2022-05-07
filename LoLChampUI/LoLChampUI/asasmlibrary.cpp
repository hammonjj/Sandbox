#include "asasmlibrary.h"

#include <iostream>
#include <stdlib.h>
#include <fstream>
#include <iterator>
#include <algorithm>
#include "toolbox.h"

using std::vector;
using std::string;
using std::ifstream;
using std::ofstream;

//Function Definitions
/***************************************************************************/
asasmlibrary::asasmlibrary(string fileName) {
		asamPrefix = "      pushstring          \"";
		asamSuffix = "\"";

		readFile(fileName);

}

/***************************************************************************/
void asasmlibrary::readFile(string fileName) {
	ifstream asamFile;
	asamFile.open(fileName);

	if(asamFile.is_open()) {
		string line;
		
		while(asamFile.good()) {
			getline(asamFile, line);
			m_asasmFile.push_back(line);
		}
	}
	asamFile.close();
}

/***************************************************************************/
void asasmlibrary::readFileResources(string fileName) {
	ifstream asamFile;
	asamFile.open(fileName);

	std::string str;

	if(asamFile.is_open()) {
		string line;
		
		while(asamFile.good()) {
			getline(asamFile, line);
			m_resourcesFile.push_back(line);
		}
	}

	else
		str = "Could not open resources file.";

	asamFile.close();
	parseResourcesFile();
}

/***************************************************************************/
void asasmlibrary::parseResourcesFile()
{
	std::string beginSearchTags = "      pushscope";
	std::string endSearchTags = "      coerce              QName(PackageNamespace(\"\"), \"Object\")";
	for(int i = 35; i < m_resourcesFile.size(); i++)
	{
		if(m_resourcesFile[i] == beginSearchTags)
		{
			if(m_resourcesFile[i+2] == endSearchTags)
			{
				return;
			}

			m_resourcesFile.erase(m_resourcesFile.begin()+i+2);
			i--;
		}
	}
	return;
}

/***************************************************************************/
void asasmlibrary::writeResourcesFile(std::vector<std::string> categoryList, std::string fileName)
{
	std::string beginSearchTags = "      pushscope";
	std::string endSearchTags = "      coerce              QName(PackageNamespace(\"\"), \"Object\")";

	std::string pushString = "      pushstring          \"champion_search_tag_";
	std::string searchTag = "      pushstring          \"";
	std::string eol = "\"";
	std::string memoryRegister = "      newobject           ";
	bool done = false;

	for(int i = 0; m_resourcesFile.size(); i++)
	{
		if(m_resourcesFile[i] == beginSearchTags)
		{
			std::string tmp = memoryRegister + std::to_string(categoryList.size());

			m_resourcesFile.insert(m_resourcesFile.begin()+i+2, tmp);
			for(int j = 0; j < categoryList.size(); j++)
			{
				std::string tmp = searchTag + categoryList[j] + eol;
				m_resourcesFile.insert(m_resourcesFile.begin()+i+2, tmp);

				tmp = pushString + categoryList[j] + eol;
				m_resourcesFile.insert(m_resourcesFile.begin()+i+2, tmp);

				if(j == categoryList.size()-1)
					done = true;
			}
		}

		if(done)
			break;
	}

	ofstream outputAsam;
	outputAsam.open(fileName);


	for(int i = 0; i < m_resourcesFile.size(); i++)
	{
		outputAsam << m_resourcesFile[i] << '\n';
	}

	outputAsam.close();
}

/***************************************************************************/

/***************************************************************************/
void asasmlibrary::writeFile(string fileName, vector<string> &fileLines) {
	ofstream outputAsam;
	outputAsam.open(fileName);


	for(int i = 0; i < fileLines.size(); i++)
	{
		outputAsam << fileLines[i] << '\n';
	}

	outputAsam.close();
}

/***************************************************************************/
void asasmlibrary::writeFile(string fileName) {
	ofstream outputAsam;
	outputAsam.open(fileName);


	for(int i = 0; i < m_asasmFile.size(); i++)
	{
		outputAsam << m_asasmFile[i] << '\n';
	}

	outputAsam.close();
}

/***************************************************************************/
void asasmlibrary::assembleFile() {
	system("res\\assembleAir.bat");
	//system("assembleResources.bat");
	//Commands are happening too fast?  Batch file works for now.
	//system("rabcdasm\\rabcasm.exe ..\\asasm\\0.0.1.30\\AirGeneratedContent-0\\AirGeneratedContent-0.main.asasm");
	//system("rabcdasm\\abcreplace.exe ..\\asasm\\0.0.1.30\\AirGeneratedContent.swf 0 ..\\asasm\\0.0.1.30\\AirGeneratedContent-0\\AirGeneratedContent-0.main.abc");
}

/***************************************************************************/
void asasmlibrary::readSearchTags(const string champName, vector<string> &searchTags) {
	string asamSearchString = "      pushstring          \"" + champName + "\"";
	string beginSTag =		  "      pushstring          \"";
	string endSTag = "\"";
	string endTags = "newarray";

	int endSTagLength = beginSTag.length();

	int lineCounter = 10; //Line displacement between champion name and search tags.
	int index = -1;

	for(int i = 0; i < m_asasmFile.size()-1; i++) 
	{ 
		if(m_asasmFile[i] == asamSearchString) 
		{
			
			//return current search tags.
			while(true) 
			{ 
				index = m_asasmFile[i+lineCounter].find(endTags);
				
				if(index != -1) 
				{
					break;
				}

				//Remove excess characters around category string
				string temp = m_asasmFile[i+lineCounter];
				temp.erase(0, endSTagLength);
				temp.erase(temp.end() - 1, temp.end());

				searchTags.push_back(temp);
				m_asasmFile.erase(m_asasmFile.begin()+i+lineCounter);
			}
		}
	}
}

/***************************************************************************/
//Read Categories
void asasmlibrary::readCategoryList(vector<string> &categoryList) {
	string categoryListBegin = "      findproperty        QName(PackageNamespace(\"\"), \"championSearchTags\")";
	string categoryListEnd = "      initproperty        QName(PackageNamespace(\"\"), \"championSearchTags\")";
	string categoryPrefix = "      pushstring          \"";
	bool complete = false;

	int prefixLength = categoryPrefix.size();
	
	for(int i = 6000;i < m_asasmFile.size(); i++) 
	{
		if(m_asasmFile[i] == categoryListBegin)
		{
			for(int j = i+1; j < m_asasmFile.size(); ) 
			{
				if(categoryListEnd == m_asasmFile[j+1])
				{
					m_asasmFile.erase(m_asasmFile.begin()+j);
					complete = true;
					break;
				}

				if(m_asasmFile[j] == "end ; class")
				{
					_ASSERT("End of file reached without finding end of categories!");
					break;
				}

				//Remove excess characters around category string
				string temp = m_asasmFile[j];
				temp.erase(0, prefixLength);
				temp.erase(temp.end() - 1, temp.end());

				categoryList.push_back(temp);
				m_asasmFile.erase(m_asasmFile.begin()+j);
			}
		}

		if(complete == true)
		{
			break;
		}
	}
}

/***************************************************************************/
void asasmlibrary::getAsasmFile(std::vector<std::string> &asamFile)
{
	asamFile = m_asasmFile;
}

/***************************************************************************/
void asasmlibrary::insertSearchTags(std::vector<LCMChampion> &championInventory)
{
	int lineCounter = 10; //Line displacement between champion name and search tags.
	string beginSTag =		  "      pushstring          \"";
	string endSTag = "\"";
	string endTags = "      newarray            ";

	for(int i = 0; i < championInventory.size(); i++)
	{
		string asamSearchString = "      pushstring          \"" + championInventory[i].getChampName() + "\"";
		for(int j = 0; j < m_asasmFile.size(); j++)
		{
			if(m_asasmFile[j] == asamSearchString)
			{
				std::vector<std::string> searchTags = championInventory[i].getSearchTags();
				m_asasmFile[j+lineCounter] = endTags + std::to_string(searchTags.size());
				for(int k = 0; k < searchTags.size(); k++)
				{
					std::string str = beginSTag + searchTags[k] + endSTag;
					m_asasmFile.insert(m_asasmFile.begin()+j+lineCounter, str);
				}
			}
		}
	}
}

/***************************************************************************/
void asasmlibrary::insertCategories(std::vector<LCMCategory> &categoryInventory)
{
	string categoryListBegin = "      findproperty        QName(PackageNamespace(\"\"), \"championSearchTags\")";
	string categoryListEnd = "      newarray            ";
	string categoryPrefix = "      pushstring          \"";
	string endCatTag = "\"";
	
	for(int i = 6000; i < m_asasmFile.size(); i++)
	{
		if(m_asasmFile[i] == categoryListBegin)
		{
			std::string sizeStr = categoryListEnd + std::to_string(categoryInventory.size());
			m_asasmFile.insert(m_asasmFile.begin()+i+1, sizeStr);
			for(int j = 0; j < categoryInventory.size(); j++)
			{
				std::string str = categoryPrefix + categoryInventory[j].getCategory() + endCatTag;
				m_asasmFile.insert(m_asasmFile.begin()+i+1, str);		
			}

		}
	}
}

/***************************************************************************/
void writeResourcesFile(std::vector<std::string> m_categoryList)
{

}