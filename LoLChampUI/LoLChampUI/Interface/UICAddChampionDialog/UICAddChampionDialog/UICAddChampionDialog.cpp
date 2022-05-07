#include "UICAddChampionDialog.h"
#include "ui_UICAddChampionDialog.h"

UICAddChampionDialog::UICAddChampionDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::UICAddChampionDialog)
{
    ui->setupUi(this);

    connect(ui->btn_cancel,SIGNAL(clicked()),this,SLOT(on_btn_cancel_clicked()));
    connect(ui->btn_accept,SIGNAL(clicked()),this,SLOT(on_btn_accept_clicked()));

	this->raise();
	this->setFocus();
}

UICAddChampionDialog::~UICAddChampionDialog()
{
    delete ui;
}

void UICAddChampionDialog::setChampionList(std::vector<std::string> comboBoxContent, bool bIsChampionList)
{
	//Clear from previous selection
	m_comboItemSelected = "";
	ui->comboBox->clear();

	m_bIsChampionList = bIsChampionList;

	//m_championList = championList;
	for(int i = 0; i < comboBoxContent.size(); i++) 
	{
		//Add vector strings to combo box.
		QString qs = comboBoxContent[i].c_str();
		ui->comboBox->addItem(qs);
	}
}

bool UICAddChampionDialog::getbChampionList()
{
	return m_bIsChampionList;
}

std::string UICAddChampionDialog::getComboContent()
{
	std::string str = m_comboItemSelected;
	return str;

}

void UICAddChampionDialog::on_btn_cancel_clicked()
{
    close();
}

void UICAddChampionDialog::on_btn_accept_clicked()
{
	QString currentText = ui->comboBox->currentText();
	m_comboItemSelected = currentText.toUtf8();
    close();
    emit newComboContent();


}
