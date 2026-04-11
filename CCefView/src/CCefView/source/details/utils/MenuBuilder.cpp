#include "MenuBuilder.h"

#include <vector>

#include <nlohmann/json.hpp>

using json = nlohmann::json;

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

  MenuItem() {}

  NLOHMANN_DEFINE_TYPE_INTRUSIVE(MenuItem,    //
                                 type,        //
                                 label,       //
                                 commandId,   //
                                 enable,      //
                                 visible,     //
                                 checked,     //
                                 groupId,     //
                                 accelerator, //
                                 subMenuData  //
  );
} MenuItem;

typedef std::vector<MenuItem> MenuData;

MenuData
ConvertCefMenuToMenuData(CefMenuModel* model)
{
  MenuData data;

  if (!model)
    return data;

  for (int i = 0; i < model->GetCount(); i++) {
    MenuItem item;
    auto type = model->GetTypeAt(i);
    item.type = (MenuItemType)(type);
    item.label = model->GetLabelAt(i).ToString();
    item.commandId = model->GetCommandIdAt(i);
    item.enable = model->IsEnabledAt(i);
    item.visible = model->IsVisibleAt(i);

    int keyCode = 0;
    bool shift = false;
    bool ctrl = false;
    bool alt = false;
    auto hasAccelerator = model->GetAcceleratorAt(i, keyCode, shift, ctrl, alt);
    if (hasAccelerator) {
      int combination = keyCode;
      // if (shift)
      //   combination += Qt::SHIFT;
      // if (ctrl)
      //   combination += Qt::CTRL;
      // if (alt)
      //   combination += Qt::ALT;
      item.accelerator = combination;
    }

    switch (type) {
      case MENUITEMTYPE_COMMAND: {
      } break;
      case MENUITEMTYPE_CHECK: {
        item.checked = model->IsCheckedAt(i);
        item.groupId = model->GetGroupIdAt(i);
      } break;
      case MENUITEMTYPE_RADIO: {
        item.checked = model->IsCheckedAt(i);
        item.groupId = model->GetGroupIdAt(i);
      } break;
      case MENUITEMTYPE_SEPARATOR: {
      } break;
      case MENUITEMTYPE_SUBMENU: {
        auto cefSubMenu = model->GetSubMenuAt(i);
        item.subMenuData = ConvertCefMenuToMenuData(cefSubMenu.get());
      } break;
      default:
        break;
    }

    data.push_back(item);
  }

  return data;
}

std::string
CreateMenuDataFromCefMenu(CefMenuModel* model)
{
  auto data = ConvertCefMenuToMenuData(model);

  return json(data).dump();
}

} // namespace MenuBuilder
