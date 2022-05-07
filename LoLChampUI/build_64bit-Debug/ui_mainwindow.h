/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 5.0.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QListView>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QRadioButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralWidget;
    QListView *list_primary;
    QListView *list_secondary;
    QPushButton *btn_AddCategory;
    QPushButton *btn_RemoveCategory;
    QPushButton *btn_AddChamp;
    QPushButton *btn_RemoveChamp;
    QRadioButton *rad_byChampion;
    QRadioButton *rad_byCategory;
    QMenuBar *menuBar;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QStringLiteral("MainWindow"));
        MainWindow->resize(262, 462);
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QStringLiteral("centralWidget"));
        list_primary = new QListView(centralWidget);
        list_primary->setObjectName(QStringLiteral("list_primary"));
        list_primary->setGeometry(QRect(20, 40, 181, 101));
        list_secondary = new QListView(centralWidget);
        list_secondary->setObjectName(QStringLiteral("list_secondary"));
        list_secondary->setGeometry(QRect(20, 150, 181, 221));
        btn_AddCategory = new QPushButton(centralWidget);
        btn_AddCategory->setObjectName(QStringLiteral("btn_AddCategory"));
        btn_AddCategory->setGeometry(QRect(210, 40, 21, 23));
        btn_AddCategory->setCheckable(false);
        btn_RemoveCategory = new QPushButton(centralWidget);
        btn_RemoveCategory->setObjectName(QStringLiteral("btn_RemoveCategory"));
        btn_RemoveCategory->setGeometry(QRect(210, 70, 21, 23));
        btn_AddChamp = new QPushButton(centralWidget);
        btn_AddChamp->setObjectName(QStringLiteral("btn_AddChamp"));
        btn_AddChamp->setGeometry(QRect(210, 150, 21, 23));
        btn_RemoveChamp = new QPushButton(centralWidget);
        btn_RemoveChamp->setObjectName(QStringLiteral("btn_RemoveChamp"));
        btn_RemoveChamp->setGeometry(QRect(210, 180, 21, 23));
        rad_byChampion = new QRadioButton(centralWidget);
        rad_byChampion->setObjectName(QStringLiteral("rad_byChampion"));
        rad_byChampion->setGeometry(QRect(20, 10, 82, 17));
        rad_byChampion->setChecked(true);
        rad_byCategory = new QRadioButton(centralWidget);
        rad_byCategory->setObjectName(QStringLiteral("rad_byCategory"));
        rad_byCategory->setGeometry(QRect(110, 10, 82, 17));
        MainWindow->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(MainWindow);
        menuBar->setObjectName(QStringLiteral("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 262, 21));
        MainWindow->setMenuBar(menuBar);
        statusBar = new QStatusBar(MainWindow);
        statusBar->setObjectName(QStringLiteral("statusBar"));
        MainWindow->setStatusBar(statusBar);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "LoL Champ Manager", 0));
        btn_AddCategory->setText(QApplication::translate("MainWindow", "+", 0));
        btn_RemoveCategory->setText(QApplication::translate("MainWindow", "-", 0));
        btn_AddChamp->setText(QApplication::translate("MainWindow", "+", 0));
        btn_RemoveChamp->setText(QApplication::translate("MainWindow", "-", 0));
        rad_byChampion->setText(QApplication::translate("MainWindow", "By Champion", 0));
        rad_byCategory->setText(QApplication::translate("MainWindow", "By Category", 0));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
