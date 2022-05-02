game 'rdr3'
fx_version 'adamant'
rdr3_warning 'I acknowledge that this is a prerelease build of RedM, and I am aware my resources *will* become incompatible once RedM ships.'

client_scripts {
  '*.Client.net.dll'
}

server_scripts {
  '*.Server.net.dll'
}

files {
  'config/**',
  'MenuAPI.dll',
  'Newtonsoft.Json.dll',
}

debug_enabled 'false'
vorp_core_csharp_new 'true'
vorp_database_resource 'ghmattimysql'