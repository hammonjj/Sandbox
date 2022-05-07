#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QStringListModel>
#include <ShObjIdl.h>

#include "LCMChampion.h"
#include "LCMCategory.h"
#include "UICAddChampionDialog.h"
#include "UICAddCategoryDialog.h"
#include "uicsettingsdialog.h"
#include "asasmlibrary.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
	void updatePrimaryList(bool bPrimaryChampionList);
	void updateSecondaryList();
	void UpdateSecondaryList_Champion(void);
    QStringList pullChampionNames();
	QStringList pullCategoryNames();
	void setChampionList(std::vector<std::string> championList);
	void setCategoryList(std::vector<std::string> categoryList);
	void setChampionInventory(std::vector<LCMChampion> &championInventory);
	void setCategoryInventory(std::vector<LCMCategory> &categoryInventory);
	void setAsasmLibrary(asasmlibrary *ChampionGeneratedData);
    ~MainWindow();
    
private slots:
    void on_addPrimary_clicked();
    void on_removePrimary_clicked();
    void on_addSecondary_clicked();
    void on_removeSecondary_clicked();
	void on_restore_clicked();
	void on_apply_clicked();
	void on_rad_byCategory_selected(bool checked);
	void on_rad_byChampion_selected(bool checked);
	void on_categoryUpdate();
	void on_settingsClicked();
	void on_close();
	void on_comboUpdate();
    void on_list_primary_changed(QModelIndex index);

private:
    Ui::MainWindow *ui;
	UICAddCategoryDialog *newPrimaryDialog;
	UICAddChampionDialog *newSecondaryDialog;
	UICSettingsDialog * settingsDialog;
    QStringListModel* m_model;
	QStringListModel* m_smodel;
	asasmlibrary* m_championGeneratedData;
    std::vector<LCMChampion> m_championInventory;
	std::vector<LCMCategory> m_categoryInventory;
	std::vector<std::string> m_championList;
	std::vector<std::string> m_categoryList;
	std::string m_lolRootDirectory;

	void getLolDirectory();
	void listByChampion(void);
	void listByCategory(void);
	void clear_secondaryList(void);
};

#endif // MAINWINDOW_H
