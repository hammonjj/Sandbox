#ifndef UICSETTINGSDIALOG_H
#define UICSETTINGSDIALOG_H

#include <QDialog>

namespace Ui {
class UICSettingsDialog;
}

class UICSettingsDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit UICSettingsDialog(QWidget *parent = 0);
    std::string getLolRoot();
	~UICSettingsDialog();

private slots:
	void on_browse();

private:
	void findLolDirectory();
	void writePathToConfig();

    Ui::UICSettingsDialog *ui;
	std::string m_lolRoot;
};

#endif // UICSETTINGSDIALOG_H
