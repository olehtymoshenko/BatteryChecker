#include "Windows.h"
#include "BatClass.h"
#include "Setupapi.h"
#include "devguid.h"

extern "C" __declspec(dllexport) bool GetBatteryInfo(BATTERY_INFORMATION* batInfo);