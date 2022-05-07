/**************************************
* Will become header file.			  *
*
*
*
***************************************/
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <iterator>
#include <algorithm>

using std::copy;
using std::cout;
using std::cin;
using std::vector;
using std::string;
using std::ifstream;
using std::ofstream;

//Constant Definitions
const string CHAMP_GENERATED_DATA_PATH = "Resources\\ASAM\\0.0.1.28\\AirGeneratedContent-0\\com\\riotgames\\platform\\gameclient\\domain";
const string CHAMP_GENERATED_DATA_ASAM = "ChampionGeneratedData.class.asasm";
const string RABCASM = "\"Resources\\RABCDAsm_v1.13\rabcasm\" \"Test Space\\Resources\\ASAM\\0.0.1.28\\AirGeneratedContent-0\\AirGeneratedContent-0.main.asasm";
const string ABCREPLACE = "\"Resources\\RABCDAsm_v1.13\rabcasm\" \"Test Space\\Resources\\ASAM\\0.0.1.28\\AirGeneratedContent.swf\" 0 \"Test Space\\Resources\\ASAM\\0.0.1.28\\AirGeneratedContent-0\\AirGeneratedContent-0.main.abc\"";

//Function Prototypes
vector<string> readFile(string fileName);
void writeFile(string fileName, vector<string> fileLines);
void assembleFile();


/***********************************
*Program Entry					   *
***********************************/
int main() {
	string concatChampionGeneratedData = CHAMP_GENERATED_DATA_PATH + "\\" + CHAMP_GENERATED_DATA_ASAM;
	vector<string> generatedChampDataAsam = readFile(concatChampionGeneratedData);

	
	
	
	
	
	
	
	writeFile(concatChampionGeneratedData, generatedChampDataAsam);

	//getchar();
	return 0;
}

//Function Definiions
vector<string> readFile(string fileName) {
	ifstream asamFile;
	asamFile.open(fileName);

	vector<string> asamLines;

	if(asamFile.is_open()) {
		string line;
		
		while(asamFile.good()) {
			getline(asamFile, line);
			asamLines.push_back(line);
		}
	}
	asamFile.close();

	return asamLines;
}

void writeFile(string fileName, vector<string> fileLines) {
	string path = "Test Space\\" + fileName;
	ofstream outputAsam;
	outputAsam.open(path);

	vector<string> locLines = fileLines;

	int x = 0;
	while(x < locLines.size()-1) {
		outputAsam << locLines[x] << '\n';
		x++;
	}

	outputAsam.close();
}

void assembleFile() {

}