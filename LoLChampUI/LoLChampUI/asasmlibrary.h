/****************************************
* File: asasmtest.h						*
* Created: 6/13/2013					*
* Author: James Hammond					*
* Comments: Doxygen						*
****************************************/

#ifndef ASASMLIBRARY_H
#define ASASMLIBRARY_H

#include <string>
#include <vector>
#include "LCMCategory.h"
#include "LCMChampion.h"

class asasmlibrary {
	private:
		std::vector<std::string> m_asasmFile;
		std::vector<std::string> m_resourcesFile;
		std::string asamPrefix;
		std::string asamSuffix;

	public:
		asasmlibrary(std::string fileName);
		void readFileResources(std::string fileName);
		void parseResourcesFile();
		void writeResourcesFile(std::vector<std::string> categoryList, std::string fileName);
		void readFile(std::string fileName); //!< Reads AMASM file and returns the file as a string vector, where each index is a line of text 
		void writeFile(std::string fileName, std::vector<std::string> &fileLines); //!< Writes ASASM file from string vector.  Will print each index on a new line.
		void writeFile(std::string fileName);
		void assembleFile(); //!< Runs Windows batch file that assembles the data using RABCDAsm v1.13
		void readSearchTags(const std::string champName, std::vector<std::string> &searchTags); //!< Stub for function that will read XML data file
		void readCategoryList(std::vector<std::string> &categoryList);
		void getAsasmFile(std::vector<std::string> &asamFile);
		void insertCategories(std::vector<LCMCategory> &categoryInventory);
		void insertSearchTags(std::vector<LCMChampion> &championInventory);
		void writeResourcesFile(std::vector<std::string> m_categoryList);
		~asasmlibrary(){;};
};


#endif