/********************************************************************************
** Form generated from reading UI file 'uicsettingsdialog.ui'
**
** Created by: Qt User Interface Compiler version 5.0.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_UICSETTINGSDIALOG_H
#define UI_UICSETTINGSDIALOG_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QDialog>
#include <QtWidgets/QHBoxLayout>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QLabel>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QVBoxLayout>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_UICSettingsDialog
{
public:
    QWidget *layoutWidget;
    QVBoxLayout *verticalLayout_2;
    QVBoxLayout *verticalLayout;
    QLabel *label;
    QHBoxLayout *horizontalLayout;
    QLineEdit *directoryEdit;
    QPushButton *btn_browse;
    QPushButton *btn_apply;

    void setupUi(QDialog *UICSettingsDialog)
    {
        if (UICSettingsDialog->objectName().isEmpty())
            UICSettingsDialog->setObjectName(QStringLiteral("UICSettingsDialog"));
        UICSettingsDialog->resize(236, 95);
        QSizePolicy sizePolicy(QSizePolicy::Preferred, QSizePolicy::Preferred);
        sizePolicy.setHorizontalStretch(236);
        sizePolicy.setVerticalStretch(95);
        sizePolicy.setHeightForWidth(UICSettingsDialog->sizePolicy().hasHeightForWidth());
        UICSettingsDialog->setSizePolicy(sizePolicy);
        UICSettingsDialog->setMinimumSize(QSize(236, 95));
        UICSettingsDialog->setMaximumSize(QSize(236, 95));
        layoutWidget = new QWidget(UICSettingsDialog);
        layoutWidget->setObjectName(QStringLiteral("layoutWidget"));
        layoutWidget->setGeometry(QRect(10, 10, 220, 77));
        verticalLayout_2 = new QVBoxLayout(layoutWidget);
        verticalLayout_2->setSpacing(6);
        verticalLayout_2->setContentsMargins(11, 11, 11, 11);
        verticalLayout_2->setObjectName(QStringLiteral("verticalLayout_2"));
        verticalLayout_2->setContentsMargins(0, 0, 0, 0);
        verticalLayout = new QVBoxLayout();
        verticalLayout->setSpacing(6);
        verticalLayout->setObjectName(QStringLiteral("verticalLayout"));
        label = new QLabel(layoutWidget);
        label->setObjectName(QStringLiteral("label"));

        verticalLayout->addWidget(label);

        horizontalLayout = new QHBoxLayout();
        horizontalLayout->setSpacing(6);
        horizontalLayout->setObjectName(QStringLiteral("horizontalLayout"));
        directoryEdit = new QLineEdit(layoutWidget);
        directoryEdit->setObjectName(QStringLiteral("directoryEdit"));

        horizontalLayout->addWidget(directoryEdit);

        btn_browse = new QPushButton(layoutWidget);
        btn_browse->setObjectName(QStringLiteral("btn_browse"));

        horizontalLayout->addWidget(btn_browse);


        verticalLayout->addLayout(horizontalLayout);


        verticalLayout_2->addLayout(verticalLayout);

        btn_apply = new QPushButton(layoutWidget);
        btn_apply->setObjectName(QStringLiteral("btn_apply"));

        verticalLayout_2->addWidget(btn_apply);


        retranslateUi(UICSettingsDialog);

        QMetaObject::connectSlotsByName(UICSettingsDialog);
    } // setupUi

    void retranslateUi(QDialog *UICSettingsDialog)
    {
        UICSettingsDialog->setWindowTitle(QApplication::translate("UICSettingsDialog", "Manager Settings", 0));
        label->setText(QApplication::translate("UICSettingsDialog", "League of Legends Root:", 0));
        directoryEdit->setText(QString());
        btn_browse->setText(QApplication::translate("UICSettingsDialog", "Browse", 0));
        btn_apply->setText(QApplication::translate("UICSettingsDialog", "Apply", 0));
    } // retranslateUi

};

namespace Ui {
    class UICSettingsDialog: public Ui_UICSettingsDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_UICSETTINGSDIALOG_H
