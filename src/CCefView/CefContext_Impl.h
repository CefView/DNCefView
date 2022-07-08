#ifndef CCEFCONTEXT_IMPL_H
#define CCEFCONTEXT_IMPL_H

#include <list>

#include "details/CCefAppDelegate.h"

#pragma once
typedef struct FolderResourceMapping
{
  std::string path;
  std::string url;
  int priority;
} FolderResourceMapping;

typedef struct ArchiveResourceMapping
{
  std::string path;
  std::string url;
  std::string password;
  int priority;
} ArchiveResourceMapping;

class CCefContext::Implementation
{
public:
  const CCefConfig* config_;
  std::list<FolderResourceMapping> folderResourceMappingList_;
  std::list<ArchiveResourceMapping> archiveResourceMappingList_;

  CefRefPtr<CefViewBrowserApp> pApp_;
  CCefAppDelegate::RefPtr pAppDelegate_;
};

#endif
