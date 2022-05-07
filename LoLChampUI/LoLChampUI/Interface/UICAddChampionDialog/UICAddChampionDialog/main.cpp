#include "UICAddChampionDialog.h"
#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    UICAddChampionDialog w;
    w.show();
    
    return a.exec();
}
