#include "UICAddCategoryDialog.h"
#include "ui_UICAddCategoryDialog.h"

UICAddCategoryDialog::UICAddCategoryDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::UICAddCategoryDialog)
{
    ui->setupUi(this);

	connect(ui->btn_cancel,SIGNAL(clicked()),this,SLOT(on_btn_cancel_clicked()));  
	connect(ui->btn_accept,SIGNAL(clicked()),this,SLOT(on_btn_accept_clicked())); 

	this->raise();
	this->setFocus();
}

UICAddCategoryDialog::~UICAddCategoryDialog()
{
    delete ui;
}

void UICAddCategoryDialog::on_btn_cancel_clicked()
{
	close();
}

void UICAddCategoryDialog::setCategoryString(std::string category) {
	m_category = category;
}

std::string UICAddCategoryDialog::getCategoryString() {
	std::string str = m_category;
	return str;

}

void UICAddCategoryDialog::on_btn_accept_clicked()
{
	m_category = "";
	QString newCategory = ui->edit_categoryText->text();
	m_category = newCategory.toUtf8();
	
	close();
	emit newCategoryString();
}
