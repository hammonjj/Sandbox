#ifndef UICADDCHAMPIONDIALOG_H
#define UICADDCHAMPIONDIALOG_H

#include <QDialog>

namespace Ui {
class UICAddChampionDialog;
}

class UICAddChampionDialog : public QDialog
{
    Q_OBJECT

signals: 
	void newComboContent();

public:
    explicit UICAddChampionDialog(QWidget *parent = 0);
	void setChampionList(std::vector<std::string> championList, bool bIsChampionList);
	bool getbChampionList();
	std::string getComboContent();
	~UICAddChampionDialog();

private slots:
    void on_btn_cancel_clicked();
    void on_btn_accept_clicked();

private:
    Ui::UICAddChampionDialog *ui;
	std::vector<std::string> m_championList;
	bool m_bIsChampionList;
	std::string m_comboItemSelected;
};

#endif // UICADDCHAMPIONDIALOG_H
