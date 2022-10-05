#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Void UnityEngine.AssetBundle::.ctor()
extern void AssetBundle__ctor_m12989CA081324BB49ED893BDA5E3B4E758D61410 (void);
// 0x00000002 T[] UnityEngine.AssetBundle::ConvertObjects(UnityEngine.Object[])
// 0x00000003 T[] UnityEngine.AssetBundle::LoadAllAssets()
// 0x00000004 UnityEngine.Object[] UnityEngine.AssetBundle::LoadAllAssets(System.Type)
extern void AssetBundle_LoadAllAssets_m0A8F41292C96F658A89B0E8D0ADB2E8395DD7F62 (void);
// 0x00000005 System.Void UnityEngine.AssetBundle::Unload(System.Boolean)
extern void AssetBundle_Unload_m0A189871E61A0D6735A2B41B3360A1F0677B636B (void);
// 0x00000006 UnityEngine.Object[] UnityEngine.AssetBundle::LoadAssetWithSubAssets_Internal(System.String,System.Type)
extern void AssetBundle_LoadAssetWithSubAssets_Internal_m14AE2B2D7696182CBDF12087E8D3FEA867290DA8 (void);
static Il2CppMethodPointer s_methodPointers[6] = 
{
	AssetBundle__ctor_m12989CA081324BB49ED893BDA5E3B4E758D61410,
	NULL,
	NULL,
	AssetBundle_LoadAllAssets_m0A8F41292C96F658A89B0E8D0ADB2E8395DD7F62,
	AssetBundle_Unload_m0A189871E61A0D6735A2B41B3360A1F0677B636B,
	AssetBundle_LoadAssetWithSubAssets_Internal_m14AE2B2D7696182CBDF12087E8D3FEA867290DA8,
};
static const int32_t s_InvokerIndices[6] = 
{
	7491,
	0,
	0,
	4406,
	5965,
	2169,
};
static const Il2CppTokenRangePair s_rgctxIndices[2] = 
{
	{ 0x06000002, { 0, 2 } },
	{ 0x06000003, { 2, 2 } },
};
extern const uint32_t g_rgctx_TU5BU5D_t66821A3C7E718EFDF5FC3BD2288BAE38816F4985;
extern const uint32_t g_rgctx_T_t3CB4386B71D70DC2B596DFC25E9D2A155CD509E6;
extern const uint32_t g_rgctx_T_t4DDCDE7720DB1E953B03BAB314A225D5D5780ED5;
extern const uint32_t g_rgctx_AssetBundle_ConvertObjects_TisT_t4DDCDE7720DB1E953B03BAB314A225D5D5780ED5_mB47003AA600C23C9DD0DAC0CB36B45856654B7A9;
static const Il2CppRGCTXDefinition s_rgctxValues[4] = 
{
	{ (Il2CppRGCTXDataType)2, (const void *)&g_rgctx_TU5BU5D_t66821A3C7E718EFDF5FC3BD2288BAE38816F4985 },
	{ (Il2CppRGCTXDataType)2, (const void *)&g_rgctx_T_t3CB4386B71D70DC2B596DFC25E9D2A155CD509E6 },
	{ (Il2CppRGCTXDataType)1, (const void *)&g_rgctx_T_t4DDCDE7720DB1E953B03BAB314A225D5D5780ED5 },
	{ (Il2CppRGCTXDataType)3, (const void *)&g_rgctx_AssetBundle_ConvertObjects_TisT_t4DDCDE7720DB1E953B03BAB314A225D5D5780ED5_mB47003AA600C23C9DD0DAC0CB36B45856654B7A9 },
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_AssetBundleModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_AssetBundleModule_CodeGenModule = 
{
	"UnityEngine.AssetBundleModule.dll",
	6,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	2,
	s_rgctxIndices,
	4,
	s_rgctxValues,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
