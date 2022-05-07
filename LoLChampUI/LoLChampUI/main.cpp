#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <vector>
#include <string>
#include <fstream>
#include <iostream>

#include "LCMChampion.h"
#include "LCMCategory.h"
#include "asasmlibrary.h"

using std::cout;
using std::string;
using std::vector;
using std::ifstream;
using std::ofstream;

string CHAMP_GENERATED_DATA_PATH = "asasm\\0.0.1.30\\AirGeneratedContent-0\\com\\riotgames\\platform\\gameclient\\domain\\ChampionGeneratedData.class.asasm";
string CHAMP_RESOURCES_DATA_PATH = "asasm\\0.0.1.30\\resources-en_US-1\\en_US$champion_search_tag_resources_properties.class.asasm";
asasmlibrary* ChampionGeneratedData;

std::vector<std::string> findLolDirectory();
void readChampList(vector<string> &champList);
void championHandler(vector<string> &championList, 	vector<string> &championSearchTags, vector<LCMChampion> &championInventory);
void categoryHandler(vector<string> &categoryList, vector<LCMCategory> &categoryInventory, vector<LCMChampion> &championInventory);

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MainWindow w;
	w.setWindowFlags(Qt::MSWindowsFixedSizeDialogHint);

	vector<string> filePaths;
    vector<string> championList;
    vector<string> championSearchTags;
	vector<string> categoryList;
	vector<string> asasmFile;
    
	vector<LCMChampion> championInventory;
    vector<LCMCategory> categoryInventory;
    
//	filePaths = findLolDirectory();

    ChampionGeneratedData = new asasmlibrary(CHAMP_GENERATED_DATA_PATH);
	ChampionGeneratedData->readFileResources(CHAMP_RESOURCES_DATA_PATH);
    championHandler(championList, championSearchTags, championInventory);
    categoryHandler(categoryList, categoryInventory, championInventory);   
 
    w.setChampionInventory(championInventory);
	w.setCategoryInventory(categoryInventory);
	w.setChampionList(championList);
	w.setCategoryList(categoryList);
	w.setAsasmLibrary(ChampionGeneratedData);
	w.show();

    return a.exec();
}

//Function Definitions
/***************************************************************************/
std::vector<std::string> findLolDirectory()
{
	std::ifstream lolConfig;
	lolConfig.open("res\\lolpath.cfg");
	std::string line;

	if(lolConfig.is_open()) {
		std::vector<std::string> filePaths;
		
		while(lolConfig.good()) {
			getline(lolConfig, line);
			filePaths.push_back(line);
		}
	lolConfig.close();
	return filePaths;
	}
}
/***************************************************************************/
void championHandler(vector<string> &championList, 	vector<string> &championSearchTags, vector<LCMChampion> &championInventory) 
{
    readChampList(championList);

    for(int i = 0; i <= championList.size(); i++) 
	{
        //Break loop at the end of championList
        if(championList[i] == "END") {
            break;
        }

        //Retrive search tags and place in vector of champions
        ChampionGeneratedData->readSearchTags(championList[i], championSearchTags);
        LCMChampion tempChampionContainer(championList[i], championSearchTags);
        championInventory.push_back(tempChampionContainer);
        championSearchTags.clear();
    }
}

/***************************************************************************/
void categoryHandler(vector<string> &categoryList, vector<LCMCategory> &categoryInventory, vector<LCMChampion> &championInventory) {
        ChampionGeneratedData->readCategoryList(categoryList);
		
		
		//Iterate through category list
		for(int i = 0; i < categoryList.size(); i++) {
			vector<string> championList;
			//Iterate through champion list
			for(int j = 0; j < championInventory.size(); j++) {
				vector<string> tempVec = championInventory[j].getSearchTags();

					//Match champion name with category and add to category list
					for(int k = 0; k < tempVec.size(); k++) {
						if(tempVec[k] == categoryList[i]) {
							string name = championInventory[j].getChampName();
							championList.push_back(name);
						}
					}
			}
			LCMCategory temp(categoryList[i], championList);
			categoryInventory.push_back(temp);
		}

}

/***************************************************************************/
void readChampList(vector<string> &champList) {
    ifstream chplst_stream;
    chplst_stream.open("res\\chplst.dat");

    if(chplst_stream.is_open()) {
        string line;

        while(chplst_stream.good()) {
            getline(chplst_stream, line);
            champList.push_back(line);
        }
    }
    chplst_stream.close();
}

