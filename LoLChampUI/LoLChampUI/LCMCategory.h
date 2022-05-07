/****************************************
* File: asasmtest.h						*
* Created: 6/13/2013					*
* Author: James Hammond					*
* Comments: Doxygen						*
****************************************/

#ifndef LCMCATEGORY_H
#define LCMCATEGORY_H

#include <vector>
#include <string>

class LCMCategory {
	private:
		std::vector<std::string> m_championList;
		std::string m_category;

	public:
		explicit LCMCategory(std::string category, std::vector<std::string> championList);
		explicit LCMCategory(std::string category);
		void setChampionList(std::vector<std::string> championList);
		void setCategory(std::string category);
		void addChampion(std::string newChampion);
		void removeChampion(std::string oldChampion);
		std::string getCategory(void);
		std::vector<std::string> getChampionList(void);
};

#endif