#include "uicsettingsdialog.h"
#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    UICSettingsDialog w;
    w.show();
    
    return a.exec();
}
