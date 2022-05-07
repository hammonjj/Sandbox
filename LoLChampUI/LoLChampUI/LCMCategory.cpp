#include "LCMCategory.h"
#include <algorithm>

//Function Definitions
/***************************************************************************/
LCMCategory::LCMCategory(std::string category, std::vector<std::string> championList) {
	m_category = category;
	m_championList = championList;
}

/***************************************************************************/
LCMCategory::LCMCategory(std::string category) {
	m_category = category;
	m_championList.clear();
}

/***************************************************************************/
void LCMCategory::setChampionList(std::vector<std::string> championList) {
	m_championList = championList;
}

/***************************************************************************/
void LCMCategory::setCategory(std::string category) {
	m_category = category;
}

/***************************************************************************/
void LCMCategory::addChampion(std::string newChampion) {
	int index;

	for(int i = 0; i < m_championList.size(); i++) {
		if(m_championList[i] == newChampion) {
			return;
		}
	}
	
	m_championList.push_back(newChampion);
	std::sort(m_championList.begin(), m_championList.end());

}

/***************************************************************************/
void LCMCategory::removeChampion(std::string oldChampion) {
	for(int i = 0; i < m_championList.size(); i++) {
		if(m_championList[i] == oldChampion) {
			m_championList.erase(m_championList.begin()+i);
			return;
		}
	}
}
/***************************************************************************/
std::string LCMCategory::getCategory(void) {
	return m_category;
}

/***************************************************************************/
std::vector<std::string> LCMCategory::getChampionList(void) {
	return m_championList;
}