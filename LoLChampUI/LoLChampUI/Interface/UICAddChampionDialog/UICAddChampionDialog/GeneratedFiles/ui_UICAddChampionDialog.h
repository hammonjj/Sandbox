/********************************************************************************
** Form generated from reading UI file 'UICAddChampionDialog.ui'
**
** Created by: Qt User Interface Compiler version 5.0.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_UICADDCHAMPIONDIALOG_H
#define UI_UICADDCHAMPIONDIALOG_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QComboBox>
#include <QtWidgets/QDialog>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QPushButton>

QT_BEGIN_NAMESPACE

class Ui_UICAddChampionDialog
{
public:
    QPushButton *btn_accept;
    QComboBox *comboBox;
    QPushButton *btn_cancel;

    void setupUi(QDialog *UICAddChampionDialog)
    {
        if (UICAddChampionDialog->objectName().isEmpty())
            UICAddChampionDialog->setObjectName(QStringLiteral("UICAddChampionDialog"));
        UICAddChampionDialog->resize(171, 73);
        QSizePolicy sizePolicy(QSizePolicy::Preferred, QSizePolicy::Preferred);
        sizePolicy.setHorizontalStretch(0);
        sizePolicy.setVerticalStretch(0);
        sizePolicy.setHeightForWidth(UICAddChampionDialog->sizePolicy().hasHeightForWidth());
        UICAddChampionDialog->setSizePolicy(sizePolicy);
        UICAddChampionDialog->setMinimumSize(QSize(171, 73));
        UICAddChampionDialog->setMaximumSize(QSize(171, 73));
        btn_accept = new QPushButton(UICAddChampionDialog);
        btn_accept->setObjectName(QStringLiteral("btn_accept"));
        btn_accept->setGeometry(QRect(24, 40, 61, 23));
        comboBox = new QComboBox(UICAddChampionDialog);
        comboBox->setObjectName(QStringLiteral("comboBox"));
        comboBox->setGeometry(QRect(20, 10, 131, 22));
        btn_cancel = new QPushButton(UICAddChampionDialog);
        btn_cancel->setObjectName(QStringLiteral("btn_cancel"));
        btn_cancel->setGeometry(QRect(86, 40, 61, 23));

        retranslateUi(UICAddChampionDialog);

        QMetaObject::connectSlotsByName(UICAddChampionDialog);
    } // setupUi

    void retranslateUi(QDialog *UICAddChampionDialog)
    {
        UICAddChampionDialog->setWindowTitle(QApplication::translate("UICAddChampionDialog", "Add Champion", 0));
        btn_accept->setText(QApplication::translate("UICAddChampionDialog", "Accept", 0));
        btn_cancel->setText(QApplication::translate("UICAddChampionDialog", "Cancel", 0));
    } // retranslateUi

};

namespace Ui {
    class UICAddChampionDialog: public Ui_UICAddChampionDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_UICADDCHAMPIONDIALOG_H
