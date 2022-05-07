/****************************************
* File: asasmtest.h						*
* Created: 6/13/2013					*
* Author: James Hammond					*
* Comments: Doxygen						*
****************************************/

#ifndef LCMChampion_H
#define LCMChampion_H

#include <vector>
#include <string>

class LCMChampion {
	private:
		std::vector<std::string> m_searchTags;
		std::string m_champName;

	public:
		LCMChampion(std::string champName, std::vector<std::string> &searchTags);
		void setSearchTags(std::vector<std::string> searchTags);
		void setChampName(std::string champName);
		void addSearchTag(std::string searchTag);
		std::string getChampName(void);
		std::vector<std::string> getSearchTags(void);
};

#endif