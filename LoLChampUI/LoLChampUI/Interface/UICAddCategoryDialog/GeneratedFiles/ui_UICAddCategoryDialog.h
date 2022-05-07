/********************************************************************************
** Form generated from reading UI file 'UICAddCategoryDialog.ui'
**
** Created by: Qt User Interface Compiler version 5.0.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_UICADDCATEGORYDIALOG_H
#define UI_UICADDCATEGORYDIALOG_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QDialog>
#include <QtWidgets/QGroupBox>
#include <QtWidgets/QHBoxLayout>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QLabel>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QVBoxLayout>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_UICAddCategoryDialog
{
public:
    QWidget *layoutWidget;
    QVBoxLayout *verticalLayout;
    QGroupBox *groupBox;
    QWidget *layoutWidget1;
    QHBoxLayout *horizontalLayout;
    QLabel *label_name;
    QLineEdit *edit_categoryText;
    QHBoxLayout *horizontalLayout_2;
    QPushButton *btn_accept;
    QPushButton *btn_cancel;

    void setupUi(QDialog *UICAddCategoryDialog)
    {
        if (UICAddCategoryDialog->objectName().isEmpty())
            UICAddCategoryDialog->setObjectName(QStringLiteral("UICAddCategoryDialog"));
        UICAddCategoryDialog->resize(235, 107);
        UICAddCategoryDialog->setMinimumSize(QSize(235, 105));
        UICAddCategoryDialog->setMaximumSize(QSize(235, 107));
        layoutWidget = new QWidget(UICAddCategoryDialog);
        layoutWidget->setObjectName(QStringLiteral("layoutWidget"));
        layoutWidget->setGeometry(QRect(10, 10, 211, 81));
        verticalLayout = new QVBoxLayout(layoutWidget);
        verticalLayout->setSpacing(6);
        verticalLayout->setContentsMargins(11, 11, 11, 11);
        verticalLayout->setObjectName(QStringLiteral("verticalLayout"));
        verticalLayout->setContentsMargins(0, 0, 0, 0);
        groupBox = new QGroupBox(layoutWidget);
        groupBox->setObjectName(QStringLiteral("groupBox"));
        layoutWidget1 = new QWidget(groupBox);
        layoutWidget1->setObjectName(QStringLiteral("layoutWidget1"));
        layoutWidget1->setGeometry(QRect(10, 10, 188, 22));
        horizontalLayout = new QHBoxLayout(layoutWidget1);
        horizontalLayout->setSpacing(6);
        horizontalLayout->setContentsMargins(11, 11, 11, 11);
        horizontalLayout->setObjectName(QStringLiteral("horizontalLayout"));
        horizontalLayout->setContentsMargins(0, 0, 0, 0);
        label_name = new QLabel(layoutWidget1);
        label_name->setObjectName(QStringLiteral("label_name"));

        horizontalLayout->addWidget(label_name);

        edit_categoryText = new QLineEdit(layoutWidget1);
        edit_categoryText->setObjectName(QStringLiteral("edit_categoryText"));

        horizontalLayout->addWidget(edit_categoryText);


        verticalLayout->addWidget(groupBox);

        horizontalLayout_2 = new QHBoxLayout();
        horizontalLayout_2->setSpacing(6);
        horizontalLayout_2->setObjectName(QStringLiteral("horizontalLayout_2"));
        btn_accept = new QPushButton(layoutWidget);
        btn_accept->setObjectName(QStringLiteral("btn_accept"));

        horizontalLayout_2->addWidget(btn_accept);

        btn_cancel = new QPushButton(layoutWidget);
        btn_cancel->setObjectName(QStringLiteral("btn_cancel"));

        horizontalLayout_2->addWidget(btn_cancel);


        verticalLayout->addLayout(horizontalLayout_2);


        retranslateUi(UICAddCategoryDialog);

        QMetaObject::connectSlotsByName(UICAddCategoryDialog);
    } // setupUi

    void retranslateUi(QDialog *UICAddCategoryDialog)
    {
        UICAddCategoryDialog->setWindowTitle(QApplication::translate("UICAddCategoryDialog", "Add New Category", 0));
        groupBox->setTitle(QString());
        label_name->setText(QApplication::translate("UICAddCategoryDialog", "Name", 0));
        btn_accept->setText(QApplication::translate("UICAddCategoryDialog", "Accept", 0));
        btn_cancel->setText(QApplication::translate("UICAddCategoryDialog", "Cancel", 0));
    } // retranslateUi

};

namespace Ui {
    class UICAddCategoryDialog: public Ui_UICAddCategoryDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_UICADDCATEGORYDIALOG_H
