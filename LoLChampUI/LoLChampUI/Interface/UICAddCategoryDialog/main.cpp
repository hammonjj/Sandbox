#include "UICAddCategoryDialog.h"
#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    UICAddCategoryDialog w;
    w.show();
    
    return a.exec();
}
