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
#include <QtWidgets/QMenu>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QRadioButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QAction *fileSettings;
    QAction *fileExit;
    QAction *actionTroubleshooting;
    QAction *actionAbout_Champion_Manager;
    QWidget *centralWidget;
    QListView *list_primary;
    QListView *list_secondary;
    QPushButton *btn_addPrimary;
    QPushButton *btn_removePrimary;
    QPushButton *btn_addSecondary;
    QPushButton *btn_removeSecondary;
    QRadioButton *rad_byChampion;
    QRadioButton *rad_byCategory;
    QPushButton *btn_apply;
    QPushButton *btn_restore;
    QMenuBar *menuBar;
    QMenu *menuFile;
    QMenu *menuHelp;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QStringLiteral("MainWindow"));
        MainWindow->resize(262, 580);
        MainWindow->setMinimumSize(QSize(262, 580));
        MainWindow->setMaximumSize(QSize(262, 580));
        QIcon icon;
        icon.addFile(QStringLiteral("x64/Release/personal.ico"), QSize(), QIcon::Normal, QIcon::Off);
        MainWindow->setWindowIcon(icon);
        MainWindow->setTabShape(QTabWidget::Rounded);
        MainWindow->setUnifiedTitleAndToolBarOnMac(false);
        fileSettings = new QAction(MainWindow);
        fileSettings->setObjectName(QStringLiteral("fileSettings"));
        fileExit = new QAction(MainWindow);
        fileExit->setObjectName(QStringLiteral("fileExit"));
        actionTroubleshooting = new QAction(MainWindow);
        actionTroubleshooting->setObjectName(QStringLiteral("actionTroubleshooting"));
        actionAbout_Champion_Manager = new QAction(MainWindow);
        actionAbout_Champion_Manager->setObjectName(QStringLiteral("actionAbout_Champion_Manager"));
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QStringLiteral("centralWidget"));
        list_primary = new QListView(centralWidget);
        list_primary->setObjectName(QStringLiteral("list_primary"));
        list_primary->setGeometry(QRect(20, 40, 181, 221));
        list_secondary = new QListView(centralWidget);
        list_secondary->setObjectName(QStringLiteral("list_secondary"));
        list_secondary->setGeometry(QRect(20, 280, 181, 221));
        btn_addPrimary = new QPushButton(centralWidget);
        btn_addPrimary->setObjectName(QStringLiteral("btn_addPrimary"));
        btn_addPrimary->setGeometry(QRect(210, 40, 21, 23));
        btn_addPrimary->setCheckable(false);
        btn_removePrimary = new QPushButton(centralWidget);
        btn_removePrimary->setObjectName(QStringLiteral("btn_removePrimary"));
        btn_removePrimary->setGeometry(QRect(210, 70, 21, 23));
        btn_addSecondary = new QPushButton(centralWidget);
        btn_addSecondary->setObjectName(QStringLiteral("btn_addSecondary"));
        btn_addSecondary->setGeometry(QRect(210, 280, 21, 23));
        btn_removeSecondary = new QPushButton(centralWidget);
        btn_removeSecondary->setObjectName(QStringLiteral("btn_removeSecondary"));
        btn_removeSecondary->setGeometry(QRect(210, 310, 21, 23));
        rad_byChampion = new QRadioButton(centralWidget);
        rad_byChampion->setObjectName(QStringLiteral("rad_byChampion"));
        rad_byChampion->setGeometry(QRect(25, 10, 82, 17));
        rad_byChampion->setChecked(true);
        rad_byCategory = new QRadioButton(centralWidget);
        rad_byCategory->setObjectName(QStringLiteral("rad_byCategory"));
        rad_byCategory->setGeometry(QRect(120, 10, 82, 17));
        btn_apply = new QPushButton(centralWidget);
        btn_apply->setObjectName(QStringLiteral("btn_apply"));
        btn_apply->setGeometry(QRect(30, 510, 75, 23));
        btn_restore = new QPushButton(centralWidget);
        btn_restore->setObjectName(QStringLiteral("btn_restore"));
        btn_restore->setGeometry(QRect(115, 510, 75, 23));
        MainWindow->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(MainWindow);
        menuBar->setObjectName(QStringLiteral("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 262, 21));
        menuFile = new QMenu(menuBar);
        menuFile->setObjectName(QStringLiteral("menuFile"));
        menuHelp = new QMenu(menuBar);
        menuHelp->setObjectName(QStringLiteral("menuHelp"));
        MainWindow->setMenuBar(menuBar);
        statusBar = new QStatusBar(MainWindow);
        statusBar->setObjectName(QStringLiteral("statusBar"));
        MainWindow->setStatusBar(statusBar);

        menuBar->addAction(menuFile->menuAction());
        menuBar->addAction(menuHelp->menuAction());
        menuFile->addAction(fileSettings);
        menuFile->addSeparator();
        menuFile->addAction(fileExit);
        menuHelp->addAction(actionTroubleshooting);
        menuHelp->addAction(actionAbout_Champion_Manager);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "LOL Champ Manager", 0));
        fileSettings->setText(QApplication::translate("MainWindow", "Settings", 0));
        fileExit->setText(QApplication::translate("MainWindow", "Exit", 0));
        actionTroubleshooting->setText(QApplication::translate("MainWindow", "Troubleshooting", 0));
        actionAbout_Champion_Manager->setText(QApplication::translate("MainWindow", "About Champion Manager", 0));
        btn_addPrimary->setText(QApplication::translate("MainWindow", "+", 0));
        btn_removePrimary->setText(QApplication::translate("MainWindow", "-", 0));
        btn_addSecondary->setText(QApplication::translate("MainWindow", "+", 0));
        btn_removeSecondary->setText(QApplication::translate("MainWindow", "-", 0));
        rad_byChampion->setText(QApplication::translate("MainWindow", "By Champion", 0));
        rad_byCategory->setText(QApplication::translate("MainWindow", "By Category", 0));
        btn_apply->setText(QApplication::translate("MainWindow", "Apply", 0));
        btn_restore->setText(QApplication::translate("MainWindow", "Restore", 0));
        menuFile->setTitle(QApplication::translate("MainWindow", "File", 0));
        menuHelp->setTitle(QApplication::translate("MainWindow", "Help", 0));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
