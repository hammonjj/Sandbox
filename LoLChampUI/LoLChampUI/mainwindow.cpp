#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "LCMChampion.h"
#include <QString>
#include <QMainWindow>
#include <QStringList>
#include <QMessageBox>
#include <string>
#include <vector>
#include <ShObjIdl.h>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
	MainWindow::setWindowIcon(QIcon("lolchampmgr.ico"));

    ui->setupUi(this);
	newPrimaryDialog = new UICAddCategoryDialog();
	newSecondaryDialog = new UICAddChampionDialog();
	settingsDialog = new UICSettingsDialog();

	ui->btn_addPrimary->setEnabled(false);
	ui->btn_removePrimary->setEnabled(false);
	ui->list_primary->setEditTriggers(QAbstractItemView::NoEditTriggers);
	ui->list_secondary->setEditTriggers(QAbstractItemView::NoEditTriggers);

	//Event Listeners
	connect(ui->list_primary,SIGNAL(clicked(const QModelIndex)),this,SLOT(on_list_primary_changed(QModelIndex)));  

	connect(ui->btn_addPrimary,SIGNAL(clicked()),this,SLOT(on_addPrimary_clicked()));  
	connect(ui->btn_removePrimary,SIGNAL(clicked()),this,SLOT(on_removePrimary_clicked()));  
	connect(ui->btn_addSecondary,SIGNAL(clicked()),this,SLOT(on_addSecondary_clicked()));  
	connect(ui->btn_removeSecondary,SIGNAL(clicked()),this,SLOT(on_removeSecondary_clicked()));  
	connect(ui->btn_apply,SIGNAL(clicked()),this,SLOT(on_apply_clicked()));  
	connect(ui->btn_restore,SIGNAL(clicked()),this,SLOT(on_restore_clicked())); 
	connect(ui->fileSettings,SIGNAL(triggered()),this,SLOT(on_settingsClicked()));
	connect(ui->fileExit,SIGNAL(triggered()),this,SLOT(on_close()));

	connect(ui->rad_byChampion,SIGNAL(toggled(bool)),this,SLOT(on_rad_byChampion_selected(bool)));  
	connect(ui->rad_byCategory,SIGNAL(toggled(bool)),this,SLOT(on_rad_byCategory_selected(bool)));  

	connect(newPrimaryDialog,SIGNAL(newCategoryString()),this,SLOT(on_categoryUpdate())); 
	connect(newSecondaryDialog, SIGNAL(newComboContent()),this,SLOT(on_comboUpdate()));

}

MainWindow::~MainWindow()
{
	delete settingsDialog;
	delete newSecondaryDialog;
	delete newPrimaryDialog;
	delete m_championGeneratedData;
    delete ui;
}

void MainWindow::on_close()
{
	close();
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::setCategoryList(std::vector<std::string> categoryList)
{
	m_categoryList = categoryList;
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::setChampionList(std::vector<std::string> championList) 
{
	m_championList = championList;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void MainWindow::on_settingsClicked()
{
	settingsDialog->show();
}
/////////////////////////////////////////////////////////////////////////////
void MainWindow::clear_secondaryList(void) 
{
	m_smodel = new QStringListModel(ui->list_secondary);
    QString str = " ";
	QStringList emptyList;
	emptyList.append(str);
    m_smodel->setStringList(emptyList);
    ui->list_secondary->setModel(m_smodel);
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::updatePrimaryList(bool bPrimaryChampionList)
{
    m_model = new QStringListModel(ui->list_primary);
	QStringList l_primaryList;

	if(ui->rad_byChampion->isChecked()) 
	{
		l_primaryList = pullChampionNames();		
	}

	else
	{
		l_primaryList = pullCategoryNames();	
		checkCategoryCount();
	}

	m_model->setStringList(l_primaryList);
	ui->list_primary->setModel(m_model);
}

/////////////////////////////////////////////////////////////////////////////
QStringList MainWindow::pullChampionNames() 
{
    std::string nameString;
	QStringList l_championNames;
    QString str;

    for(int i = 0; i < m_championInventory.size() ; i++) {
        //Convert from string to qstring
        nameString = m_championInventory[i].getChampName();
        str = QString::fromUtf8(nameString.c_str());

        //Add converted name to m_championNames
        l_championNames.append(str);
    }

	return l_championNames;
}

/////////////////////////////////////////////////////////////////////////////
QStringList MainWindow::pullCategoryNames() 
{
    std::string categoryString;
	QStringList l_categoryNames;
    QString str;

    for(int i = 0; i < m_categoryInventory.size() ; i++) {
        //Convert from string to qstring
        categoryString = m_categoryInventory[i].getCategory();
        str = QString::fromUtf8(categoryString.c_str());

        //Add converted name to m_championNames
        l_categoryNames.append(str);
    }

	return l_categoryNames;
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::setChampionInventory(std::vector<LCMChampion> &championInventory) 
{
	 m_championInventory = championInventory;
	 updatePrimaryList(true);
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::setCategoryInventory(std::vector<LCMCategory> &categoryInventory) 
{
	m_categoryInventory = categoryInventory;
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::listByChampion(void) 
{
	ui->list_primary->setModel(m_model);
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::listByCategory(void) 
{
	ui->list_secondary->setModel(m_model);
}

//Event Listener Methods
/**********************************************************************************************************/
void MainWindow::on_categoryUpdate() 
{
	std::string category = newPrimaryDialog->getCategoryString();

	//Prevent duplicate entries
	for(int i = 0; i < m_categoryList.size(); i++)
	{
		if(category == m_categoryList[i])
		{
			return;
		}
	}

	m_categoryList.push_back(category);
	LCMCategory tempCat(category);
	m_categoryInventory.push_back(tempCat);

	updatePrimaryList(false);
	clear_secondaryList();
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_comboUpdate() 
{
	std::string comboContent = newSecondaryDialog->getComboContent(); //By Category Selected = Champion Name; By Champion Selected = Category

	//By Category Selected = Category; By Champion Selected = Champion Name
	std::string itemSelectedPrimary = ui->list_primary->selectionModel()->currentIndex().data().toString().toUtf8();

	bool bIsChampionList = newSecondaryDialog->getbChampionList();
	if(bIsChampionList) //Add Champion to Category - By Category Selected
	{
		for(int i = 0; i < m_categoryInventory.size(); i++)
		{
			if(itemSelectedPrimary == m_categoryInventory[i].getCategory())
			{
				m_categoryInventory[i].addChampion(comboContent);
				break;
			}
		}

		//Add Category to Champion
		for(int i = 0; i < m_championInventory.size(); i++)
		{
			if(comboContent == m_championInventory[i].getChampName())
			{
				m_championInventory[i].addSearchTag(itemSelectedPrimary);
				break;
			}
		}
	}	

	else //Add Category to Champion - By Champion Selected
	{
		for(int i = 0; i < m_championInventory.size(); i++)
		{
			if(itemSelectedPrimary == m_championInventory[i].getChampName())
			{
				m_championInventory[i].addSearchTag(comboContent);
				break;
			}
		}

		//Add Champion to Category
		for(int i = 0; i < m_categoryInventory.size(); i++)
		{
			if(comboContent == m_categoryInventory[i].getCategory())
			{
				m_categoryInventory[i].addChampion(itemSelectedPrimary);
				break;
			}
		}
	}	

	updateSecondaryList();
}
/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_addPrimary_clicked()
{
	if(ui->rad_byCategory->isChecked()) 
		newPrimaryDialog->show();

	else //Currently disabled so users can't remove champions
		return;
}

///////////////////////////////////////////////////////////////////////////// /* Add removeItem to LCMCategory and LCMChampion */
void MainWindow::on_removePrimary_clicked()
{
	if(ui->rad_byCategory->isChecked()) {
		//Remove Category from list
		QModelIndex itemSelected = ui->list_primary->selectionModel()->currentIndex();

		bool valid = itemSelected.isValid();
		if(valid == false) return;

		std::string itemSelectedString = itemSelected.data().toString().toUtf8();

		int row = itemSelected.row();
		
		m_categoryInventory.erase(m_categoryInventory.begin()+row, m_categoryInventory.begin()+row+1); //Member to add

		//Remove champions from category
		for(int i = 0; i < m_championInventory.size(); i++) {
			std::vector<std::string> tempTags = m_championInventory[i].getSearchTags();
				for(int j = 0; j < tempTags.size(); j++) {
					if(itemSelectedString == tempTags[j]) {
						tempTags.erase(tempTags.begin()+j, tempTags.begin()+j+1);
						m_championInventory[i].setSearchTags(tempTags);
					}
				}
		}

		//Refresh View
		updatePrimaryList(false);
		clear_secondaryList();
	}

	else //Currently disabled so users can't remove champions
		return;
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_addSecondary_clicked()
{
	//Create addChampionDialog box
	QModelIndex itemSelected = ui->list_primary->selectionModel()->currentIndex();
	std::string itemSelectedString = itemSelected.data().toString().toUtf8();

	if(ui->rad_byCategory->isChecked()) {
		//Get champions assigned to current category and send them to UICAddChampionDialog
		for(int i = 0; i < m_categoryInventory.size(); i++)
		{
			if(itemSelectedString == m_categoryInventory[i].getCategory())
			{
				std::vector<std::string> championList = m_categoryInventory[i].getChampionList();

				//Get new vector of champions not in the current category from the total pool
				std::sort(m_championList.begin(), m_championList.end());
				std::sort(championList.begin(), championList.end());
				std::vector<std::string> differenceList;
				std::set_difference(m_championList.begin(), m_championList.end(), championList.begin(), championList.end(), std::back_inserter(differenceList));

				bool bIsChampionList = true;
				newSecondaryDialog->setChampionList(differenceList, bIsChampionList);
				newSecondaryDialog->show();
				break;
			}
		}
	}

	if(ui->rad_byChampion->isChecked()) 
	{
		for(int i = 0; i < m_championInventory.size(); i++)
		{
			if(itemSelectedString == m_championInventory[i].getChampName())
			{
				std::vector<std::string> categoryList = m_championInventory[i].getSearchTags();

				//Get new vector of champions not in the current category from the total pool
				std::sort(m_categoryList.begin(), m_categoryList.end());
				std::sort(categoryList.begin(), categoryList.end());
				std::vector<std::string> differenceList;
				std::set_difference(m_categoryList.begin(), m_categoryList.end(), categoryList.begin(), categoryList.end(), std::back_inserter(differenceList));

				bool bIsChampionList = false;
				newSecondaryDialog->setChampionList(differenceList, bIsChampionList);
				newSecondaryDialog->show();
				break;
			}
		}
	}
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_removeSecondary_clicked()
{
	//Remove champion from category
	if(ui->rad_byCategory->isChecked()) 
	{
		QModelIndex categorySelected = ui->list_primary->selectionModel()->currentIndex();
		QModelIndex championSelected = ui->list_secondary->selectionModel()->currentIndex();

		bool champValid = championSelected.isValid();
		bool catValid = categorySelected.isValid();
		if(champValid == false || catValid == false) return;

		std::string championSelectedString = championSelected.data().toString().toUtf8();
		int champRow = championSelected.row();
		int categoryRow = categorySelected.row();
		
		std::string categorySelectedString = categorySelected.data().toString().toUtf8();

		//Remove champion from m_categoryInventory
		for(int i = 0; i < m_categoryInventory.size(); i++) 
		{
			if(categorySelectedString == m_categoryInventory[i].getCategory()) 
			{
				std::vector<std::string> championList = m_categoryInventory[i].getChampionList();
				for(int j = 0; j < championList.size(); j++) 
				{
					if(championSelectedString == championList[j]) 
					{
						championList.erase(championList.begin()+j, championList.begin()+j+1);
						m_categoryInventory[i].setChampionList(championList);
					}
				}
			}
		}
		
		bool complete = false;

		//Remove category from m_championInventory
		for(int i = 0; i < m_championInventory.size(); i++) 
		{
			if(complete == true) { break; }

			if(championSelectedString == m_championInventory[i].getChampName()) 
			{
				std::vector<std::string> categoryList = m_championInventory[i].getSearchTags();
				for(int j = 0; j < categoryList.size(); j++) 
				{
					if(categorySelectedString == categoryList[j]) 
					{
						categoryList.erase(categoryList.begin()+j, categoryList.begin()+j+1);
						m_championInventory[i].setSearchTags(categoryList);
						complete = true;
					}
				}
			}
		}
	}	

	//Remove category from champion
	if(ui->rad_byChampion->isChecked()) 
	{
		QModelIndex championSelected = ui->list_primary->selectionModel()->currentIndex();
		QModelIndex categorySelected = ui->list_secondary->selectionModel()->currentIndex();

		bool champValid = championSelected.isValid();
		bool catValid = categorySelected.isValid();
		
		if(champValid == false || catValid == false) return;

		std::string championSelectedString = championSelected.data().toString().toUtf8();
		int champRow = championSelected.row();
		int categoryRow = categorySelected.row();
		
		std::string categorySelectedString = categorySelected.data().toString().toUtf8();
			
		bool complete = false;	
		
		//Remove category from m_championInventory
		for(int i = 0; i < m_championInventory.size(); i++) 
		{
			if(complete == true) { break; }
		
			if(championSelectedString == m_championInventory[i].getChampName()) 
			{
				std::vector<std::string> categoryList = m_championInventory[i].getSearchTags();
				for(int j = 0; j < categoryList.size(); j++) 
				{
					if(categorySelectedString == categoryList[j]) 
					{
						categoryList.erase(categoryList.begin()+j, categoryList.begin()+j+1);
						m_championInventory[i].setSearchTags(categoryList);
						complete = true;
					}
				}
			}
		}

		//Remove champion from m_categoryInventory
		for(int i = 0; i < m_categoryInventory.size(); i++) 
		{
			if(categorySelectedString == m_categoryInventory[i].getCategory()) 
			{
				std::vector<std::string> championList = m_categoryInventory[i].getChampionList();
				for(int j = 0; j < championList.size(); j++) 
				{
					if(championSelectedString == championList[j]) 
					{
						championList.erase(championList.begin()+j, championList.begin()+j+1);
						m_categoryInventory[i].setChampionList(championList);
					}
				}
			}
		}
	}	

	updateSecondaryList();
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_restore_clicked() 
{
	//Restore original riot .swf file
	getLolDirectory();

	//Delete/Copy resources-en_US
	std::string airGeneratedRootSuffix = "\\League of Legends\\RADS\\projects\\lol_air_client\\releases\\0.0.1.30\\deploy\\assets\\swfs\\AirGeneratedContent.swf";
	std::string path = m_lolRootDirectory + airGeneratedRootSuffix;
	std::wstring stemp = std::wstring(path.begin(), path.end());

	int successAirD = DeleteFile(stemp.c_str());
	int successAir = CopyFile(L"res\\AirGeneratedContent.swf", stemp.c_str(), false);
	
	//Delete/Copy resources-en_US
	std::string resourcesUSSuffix = "\\League of Legends\\RADS\\projects\\lol_air_client\\releases\\0.0.1.30\\deploy\\assets\\locale\\Game\\resources-en_US.swf";
	path = m_lolRootDirectory + resourcesUSSuffix;
	std::wstring stemp = std::wstring(path.begin(), path.end());
	
	int successResourcesD = DeleteFile(stemp.c_str());
	int successResources = CopyFile(L"res\\resources-en_US.swf", stemp.c_str(), false);

	QMessageBox messageBox;
	std:: string infoMsg;

	if(successAir & successResources)
		infoMsg = "Successfully updated LoL game files.";

	else
		infoMsg = "There was a problem copying your game files.";

		messageBox.information(0,"LoL Champ Manager", infoMsg.c_str());
		messageBox.setFixedSize(500,200);
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_apply_clicked() 
{
	m_championGeneratedData->insertSearchTags(m_championInventory);
	m_championGeneratedData->insertCategories(m_categoryInventory);
	m_championGeneratedData->writeFile("asasm\\0.0.1.30\\AirGeneratedContent-0\\com\\riotgames\\platform\\gameclient\\domain\\ChampionGeneratedData.class.asasm");
	m_championGeneratedData->writeResourcesFile(m_categoryList, "asasm\\0.0.1.30\\resources-en_US-1\\en_US$champion_search_tag_resources_properties.class.asasm");
	m_championGeneratedData->assembleFile();

	getLolDirectory();

	//Delete/Copy resources-en_US
	std::string airGeneratedRootSuffix = "\\League of Legends\\RADS\\projects\\lol_air_client\\releases\\0.0.1.30\\deploy\\assets\\swfs\\AirGeneratedContent.swf";
	std::string path = m_lolRootDirectory + airGeneratedRootSuffix;
	std::wstring stemp = std::wstring(path.begin(), path.end());

	int successAirD = DeleteFile(stemp.c_str());
	int successAir = CopyFile(L"asasm\\0.0.1.30\\AirGeneratedContent.swf", stemp.c_str(), false);
	
	//Delete/Copy resources-en_US
	std::string resourcesUSSuffix = "\\League of Legends\\RADS\\projects\\lol_air_client\\releases\\0.0.1.30\\deploy\\assets\\locale\\Game\\resources-en_US.swf";
	path = m_lolRootDirectory + resourcesUSSuffix;
	std::wstring stemp = std::wstring(path.begin(), path.end());
	
	int successResourcesD = DeleteFile(stemp.c_str());
	int successResources = CopyFile(L"asasm\\0.0.1.30\\resources-en_US.swf", stemp.c_str(), false);

	QMessageBox messageBox;
	std:: string infoMsg;

	if(successAir & successResources)
		infoMsg = "Successfully updated LoL game files.";

	else
		infoMsg = "There was a problem copying your game files.";

		messageBox.information(0,"LoL Champ Manager", infoMsg.c_str());
		messageBox.setFixedSize(500,200);
		
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::checkCategoryCount() //Ensure someone can't add more than 12 categories or remove when there are 0
{
		int categoryCount = ui->list_primary->rowCount();
		
		if(categoryCount > 11)
			ui->btn_addPrimary->setEnabled(false);
		
		else
			ui->btn_addPrimary->setEnabled(true);
		
		if(categoryCount == 0)
			ui->btn_removePrimary->setEnabled(false);

		else
			ui->btn_removePrimary->setEnabled(true);
}
/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_rad_byCategory_selected(bool checked) 
{
	if(checked) {
		checkCategoryCount();
			
		clear_secondaryList();
		updatePrimaryList(false);
	}
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_rad_byChampion_selected(bool checked) 
{
	if(checked) {
		ui->btn_addPrimary->setEnabled(false);
		ui->btn_removePrimary->setEnabled(false);

		clear_secondaryList();
		updatePrimaryList(true);
	}
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::on_list_primary_changed(QModelIndex index)
{
	bool byChampionToggled = ui->rad_byChampion->isChecked();

	if(ui->rad_byChampion->isChecked()) 
	{
		int row = index.row();
		m_smodel = new QStringListModel(ui->list_secondary);

		QStringList searchTagList;

		std::vector<std::string> searchTag = m_championInventory[row].getSearchTags();

		for(int i = 0; i < searchTag.size(); i++) 
		{
			QString str = QString::fromUtf8(searchTag[i].c_str());
			searchTagList.append(str);
		}

		//Add converted name to model
		m_smodel->setStringList(searchTagList);
		ui->list_secondary->setModel(m_smodel);
	}

	else 
	{
		int row = index.row();
		m_smodel = new QStringListModel(ui->list_secondary);

		QStringList championList;

		std::vector<std::string> championList_s = m_categoryInventory[row].getChampionList();

		for(int i = 0; i < championList_s.size(); i++) 
		{
			QString str = QString::fromUtf8(championList_s[i].c_str());
			championList.append(str);
		}

		//Add converted name to model
		m_smodel->setStringList(championList);
		ui->list_secondary->setModel(m_smodel);
	}
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::updateSecondaryList()
{
	int row = ui->list_primary->selectionModel()->currentIndex().row();
	m_smodel = new QStringListModel(ui->list_secondary);
	std::vector<std::string> secondaryListItems;
	QStringList secondaryList;

	if(ui->rad_byChampion->isChecked()) 
	{
		secondaryListItems = m_championInventory[row].getSearchTags();
	}

	else
	{
		secondaryListItems = m_categoryInventory[row].getChampionList();
	}


	for(int i = 0; i < secondaryListItems.size(); i++) 
	{
		QString str = QString::fromUtf8(secondaryListItems[i].c_str());
		secondaryList.append(str);
	}

	//Add converted name to model
	m_smodel->setStringList(secondaryList);
	ui->list_secondary->setModel(m_smodel);
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::setAsasmLibrary(asasmlibrary *ChampionGeneratedData)
{
	m_championGeneratedData = ChampionGeneratedData;
}

/////////////////////////////////////////////////////////////////////////////
void MainWindow::getLolDirectory()
{
	m_lolRootDirectory = settingsDialog->getLolRoot();
}
/**********************************************************************************************************/