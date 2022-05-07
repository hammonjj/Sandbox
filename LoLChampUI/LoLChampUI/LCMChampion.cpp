#include "LCMChampion.h"
#include <algorithm>



//Function Definitions
/***************************************************************************/
LCMChampion::LCMChampion(std::string champName, std::vector<std::string> &searchTags) 
{
	m_champName = champName;
	m_searchTags = searchTags;
}

/***************************************************************************/
void LCMChampion::setSearchTags(std::vector<std::string> searchTags) 
{
	m_searchTags = searchTags;
}

/***************************************************************************/
void LCMChampion::setChampName(std::string champName) 
{
	m_champName = champName;
}

/***************************************************************************/
void LCMChampion::addSearchTag(std::string searchTag)
{
	for(int i = 0; i < m_searchTags.size(); i++) {
		if(m_searchTags[i] == searchTag) {
			return;
		}
	}

	m_searchTags.push_back(searchTag);
	std::sort(m_searchTags.begin(), m_searchTags.end());
}
/***************************************************************************/
std::string LCMChampion::getChampName() 
{
	return m_champName;
}

/***************************************************************************/
std::vector<std::string> LCMChampion::getSearchTags() 
{
	return m_searchTags;
}
