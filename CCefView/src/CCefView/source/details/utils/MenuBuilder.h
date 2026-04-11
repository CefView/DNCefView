#pragma once

#include <string>

#include <include/cef_app.h>

namespace MenuBuilder {

std::string
CreateMenuDataFromCefMenu(CefMenuModel* model);

// void
// BuildQtMenuFromMenuData(QMenu* menu, const MenuData& data);
}; // namespace MenuBuilder
