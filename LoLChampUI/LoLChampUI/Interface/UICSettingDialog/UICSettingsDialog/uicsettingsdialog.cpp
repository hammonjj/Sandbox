#include "uicsettingsdialog.h"
#include "ui_uicsettingsdialog.h"
#include "qfiledialog.h"
#include <iostream>
#include <fstream>

UICSettingsDialog::UICSettingsDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::UICSettingsDialog)
{
    ui->setupUi(this);
	findLolDirectory();

	connect(ui->btn_browse,SIGNAL(clicked()),this,SLOT(on_browse()));  
	connect(ui->btn_apply,SIGNAL(clicked()),this,SLOT(on_apply()));  
}

UICSettingsDialog::~UICSettingsDialog()
{
    delete ui;
}

void UICSettingsDialog::on_browse()
{
	QString lolRoot = QFileDialog::getExistingDirectory(this, tr("Open Directory"),"C:\\",QFileDialog::ShowDirsOnly | QFileDialog::DontResolveSymlinks);
	ui->directoryEdit->setText(lolRoot);
}

void UICSettingsDialog::on_apply()
{
	QString lolRoot = ui->directoryEdit->text();
	
	m_lolRoot = lolRoot.toUtf8();
	writePathToConfig();
	close();
}

void UICSettingsDialog::findLolDirectory()
{
	std::ifstream lolConfig;
	lolConfig.open("res\\lolpath.cfg");

	if(lolConfig.is_open()) {
			getline(lolConfig, m_lolRoot);
			lolConfig.close();
		}

	QString lolFilePath = m_lolRoot.c_str();
	ui->directoryEdit->setText(lolFilePath);
}

std::string UICSettingsDialog::getLolRoot()
{
	return m_lolRoot;
}

void UICSettingsDialog::writePathToConfig()
{
	std::ofstream lolConfig;
	lolConfig.open("res\\lolpath.cfg");

	lolConfig << m_lolRoot;

	lolConfig.close();
}