#pragma once

#include <string>
#include <vector>

#include <include/cef_app.h>

namespace MenuBuilder {
/// <summary>
///
/// </summary>
typedef enum MenuItemType
{
  kMeueItemTypeNone,
  kMeueItemTypeCommand,
  kMeueItemTypeCheck,
  kMeueItemTypeRadio,
  kMeueItemTypeSeparator,
  kMeueItemTypeSubMenu,
} MenuItemType;

/// <summary>
///
/// </summary>
typedef struct MenuItem
{
  MenuItemType type = kMeueItemTypeNone;
  std::string label;
  int commandId = 0;
  bool enable = false;
  bool visible = false;
  bool checked = false;
  int groupId = -1;
  int accelerator = -1;
  std::vector<MenuItem> subMenuData;
} MenuItem;

typedef std::vector<MenuItem> MenuData;

MenuData
CreateMenuDataFromCefMenu(CefMenuModel* model);

//void
//BuildQtMenuFromMenuData(QMenu* menu, const MenuData& data);
}; // namespace MenuBuilder
