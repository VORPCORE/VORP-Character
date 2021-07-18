
function contains(table, element)
    for k, v in pairs(table) do
    if k == element then
    return true
        end
    end
  return false
  end
Citizen.CreateThread(function() 
    exports.ghmattimysql:execute('SELECT * FROM characters', {}, function(result)
        if result[1] ~= nil then 
            for i=1, #result, 1 do
                local id = result[i].identifier
                local charid = result[i].charidentifier
                local comps = json.decode(result[i].compPlayer)
                    if not contains(comps, "Spats") then 
                        comps.Spats = -1
                    end
                    if not contains(comps, "Gauntlets") then 
                        comps.Gauntlets = -1
                    end
                    if not contains(comps, "Loadouts") then 
                        comps.Loadouts = -1
                    end
                    if not contains(comps, "Accessories") then 
                        comps.Accessories = -1
                    end
                    if not contains(comps, "Satchels") then 
                        comps.Satchels = -1
                    end
                    if not contains(comps, "GunbeltAccs") then 
                        comps.GunbeltAccs = -1
                    end
                local Parameters = { ['identifier'] = id, ['charidentifier'] = charid, ['compPlayer'] = json.encode(comps) } 
                exports.ghmattimysql:execute("UPDATE characters Set compPlayer=@compPlayer WHERE identifier=@identifier AND charidentifier=@charidentifier ", Parameters)
            end
        end
    end)
    exports.ghmattimysql:execute('SELECT * FROM outfits', {}, function(result)
        if result[1] ~= nil then 
            for i=1, #result, 1 do
                local id = result[i].identifier
                local charid = result[i].charidentifier
                local comps = json.decode(result[i].comps)
                    if not contains(comps, "Spats") then 
                        comps.Spats = -1
                    end
                    if not contains(comps, "Gauntlets") then 
                        comps.Gauntlets = -1
                    end
                    if not contains(comps, "Loadouts") then 
                        comps.Loadouts = -1
                    end
                    if not contains(comps, "Accessories") then 
                        comps.Accessories = -1
                    end
                    if not contains(comps, "Satchels") then 
                        comps.Satchels = -1
                    end
                    if not contains(comps, "GunbeltAccs") then 
                        comps.GunbeltAccs = -1
                    end
                local Parameters = { ['identifier'] = id, ['charidentifier'] = charid, ['comps'] = json.encode(comps) } 
                exports.ghmattimysql:execute("UPDATE outfits Set comps=@comps WHERE identifier=@identifier AND charidentifier=@charidentifier ", Parameters)
            end
        end
    end)
    print("Delete this Code!!!!")
    print("DO IT NOW!!!!")
    print("All your new clothing is updated in the compPlayer table of characters")
    print("All your new clothing is updated in the comps table of outfits")
    print("----------------------------------------------------------------------")
    print("Now install the new vorp_character and vorp_clothingstore")
    print("Restart your server!!!!")
    print("You are now updated")
end)