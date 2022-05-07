#ifndef UICADDCATEGORYDIALOG_H
#define UICADDCATEGORYDIALOG_H

#include <QDialog>
#include <string>

namespace Ui {
class UICAddCategoryDialog;
}

class UICAddCategoryDialog : public QDialog
{
    Q_OBJECT

signals: 
	void newCategoryString();

public:
    explicit UICAddCategoryDialog(QWidget *parent = 0);
	void setCategoryString(std::string category);
	std::string getCategoryString();
    ~UICAddCategoryDialog();
    
private slots:
    void on_btn_accept_clicked();
    void on_btn_cancel_clicked();

private:
	std::string m_category;
    Ui::UICAddCategoryDialog *ui;
};

#endif // UICADDCATEGORYDIALOG_H
