// Common.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"


FUNC_HELLO_IMPL(Common);

ObjectContext * const ObjectContext::PInstance = new ObjectContext();