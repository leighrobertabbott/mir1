# Legend of Mir 1 - Comprehensive Implementation Plan

## Overview
This document outlines the exhaustive implementation plan for all TODO items in the Legend of Mir 1 project.

---

## 1. Attribute System (Replace Classes) ⚠️ CRITICAL FOUNDATION

### Current State
- Classes (Warrior/Wizard/Taoist) are hardcoded throughout the system
- BaseStats calculates stats based on MirClass
- Character creation requires class selection
- Attributes exist but don't affect base stats

### Implementation Steps

#### 1.1 Remove Class Requirement from Character Creation
- **Files**: `Client/MirScenes/Dialogs/NewCharacterDialog.cs`, `Shared/ClientPackets.cs`, `Server/MirDatabase/CharacterInfo.cs`
- Remove class selection UI
- Make Class field nullable or use a default
- Update NewCharacter packet to not require class

#### 1.2 Convert BaseStats to Attribute-Based System
- **Files**: `Shared/BaseStats.cs`, `Server/MirObjects/HumanObject.cs`
- Create attribute-to-stat mapping system
- Primary attributes (Fire/Water/Electric/Ground/Good/Evil/Yin/Yang) determine base stats
- Secondary attributes (Health/Mana/Attack/Magic/Agility/Defence) provide bonuses
- Formula: Base stat = f(primary attributes) + f(secondary attributes) + level bonuses

#### 1.3 Update All Class References
- **Files**: All files using `MirClass` checks
- Replace class checks with attribute checks
- Update item requirements to use attributes instead of classes
- Update skill/spell requirements to use attributes

#### 1.4 Attribute-Based Stat Calculation
- **Files**: `Server/MirObjects/HumanObject.cs::RefreshLevelStats()`
- Calculate stats based on attribute levels and points
- Example: High Fire attribute = higher MC, High Ground = higher AC

---

## 2. Good/Evil System ✅ PARTIALLY IMPLEMENTED

### Current State
- Good/Evil levels added to CharacterInfo
- Save/Load implemented
- Need: Item requirements, Cave requirements, NPC requirements, Monster EXP

### Implementation Steps

#### 2.1 Good/Evil Level Calculation
- **Files**: `Server/MirObjects/PlayerObject.cs`
- Add methods: `GainGoodExperience()`, `GainEvilExperience()`
- Calculate level based on experience (similar to character level)
- Send Good/Evil level updates to client

#### 2.2 Item Good/Evil Requirements
- **Files**: `Shared/Data/ItemData.cs`, `Server/MirObjects/HumanObject.cs`
- Add `RequiredGoodLevel`, `RequiredEvilLevel` to ItemInfo
- Check requirements in `CanEquip()` method
- Display requirements in item tooltips

#### 2.3 Cave Good/Evil Requirements
- **Files**: `Server/MirDatabase/MapInfo.cs`, `Server/MirObjects/PlayerObject.cs`
- Add `RequiredGoodLevel`, `RequiredEvilLevel` to MapInfo
- Check on map entry
- Block entry if requirements not met

#### 2.4 NPC Good/Evil Requirements
- **Files**: `Server/MirDatabase/NPCInfo.cs`, `Server/MirObjects/NPCObject.cs`
- Add requirements to NPCInfo
- Check when interacting with NPCs
- Show appropriate messages

#### 2.5 Monster Good/Evil EXP Distribution
- **Files**: `Server/MirObjects/MonsterObject.cs`, `Server/MirObjects/PlayerObject.cs`
- Monsters have Good/Evil alignment
- When killed, distribute EXP to appropriate alignment
- Good monsters → Good EXP, Evil monsters → Evil EXP

#### 2.6 Good/Evil UI Display
- **Files**: `Client/MirScenes/Dialogs/MainDialogs.cs`, `Client/MirScenes/Dialogs/CharacterDialog.cs`
- Display Good/Evil levels in character dialog
- Show Good/Evil experience progress

---

## 3. Castle System 🏰 NEW SYSTEM

### Implementation Steps

#### 3.1 Castle Data Structure
- **Files**: `Server/MirDatabase/CastleInfo.cs` (NEW)
- Create CastleInfo class with:
  - Index, Name, MapIndex, Location
  - OwnerGuildIndex, OwnerGuildName
  - WarSchedule, WarStatus
  - PrisonLocations, GuardLocations
  - TaxRate, Treasury

#### 3.2 Castle Ownership
- **Files**: `Server/MirObjects/GuildObject.cs`, `Server/MirObjects/PlayerObject.cs`
- Add `OwnedCastle` to GuildInfo
- Castle capture mechanics
- Ownership transfer on capture

#### 3.3 Castle War System
- **Files**: `Server/MirObjects/CastleWar.cs` (NEW)
- War scheduling system
- War duration and rules
- Capture conditions
- War notifications

#### 3.4 Castle Guild Panel
- **Files**: `Client/MirScenes/Dialogs/CastleDialog.cs` (NEW)
- Display castle information
- Show ownership, tax rate, treasury
- War status and schedule
- Guild member management for castle

#### 3.5 Castle Prisons
- **Files**: `Server/MirDatabase/CastleInfo.cs`, `Server/MirObjects/PlayerObject.cs`
- Prison locations in castle
- Imprisonment mechanics
- Release conditions
- Prison UI

#### 3.6 Castle Guards & NPCs
- **Files**: `Server/MirDatabase/CastleInfo.cs`
- Guard spawn points
- Guard AI (see section 4)
- Castle-specific NPCs

---

## 4. Archer Guard AI 🎯

### Implementation Steps

#### 4.1 Archer AI for Red Players
- **Files**: `Server/MirObjects/Monsters/ArcherGuard.cs` (NEW)
- Detect red/brown name players
- Attack red players on sight
- Range-based attacks
- Aggro management

#### 4.2 Guard AI for Monsters
- **Files**: `Server/MirObjects/Monsters/Guard.cs` (NEW)
- Detect monsters in range
- Attack monsters
- Patrol behavior
- Return to position after combat

#### 4.3 Guard AI for Pets
- **Files**: `Server/MirObjects/Monsters/Guard.cs`
- Detect player pets
- Attack hostile pets
- Ignore friendly pets

#### 4.4 Guard Spawn System
- **Files**: `Server/MirDatabase/CastleInfo.cs`
- Spawn guards at castle locations
- Guard respawn mechanics
- Guard level/equipment

---

## 5. TT System (Town Teleport) 📍

### Implementation Steps

#### 5.1 Castle Detection System
- **Files**: `Server/MirObjects/PlayerObject.cs`
- Detect nearest castle based on player facing direction
- Calculate distance to castles
- Determine facing direction

#### 5.2 Town Teleport Implementation
- **Files**: `Server/MirObjects/PlayerObject.cs`, `Shared/ServerPackets.cs`
- Add TownTeleport method
- Teleport to nearest castle entrance
- Cooldown system
- Cost system (if needed)

#### 5.3 Town Teleport UI
- **Files**: `Client/MirScenes/Dialogs/MainDialogs.cs`
- Add Town Teleport button
- Show available destinations
- Display cooldown/cost

---

## 6. Ranged AI for Monsters 🏹

### Implementation Steps

#### 6.1 Ranged Attack Detection
- **Files**: `Server/MirObjects/MonsterObject.cs`
- Check if monster has ranged attack capability
- Determine attack range
- Line of sight checking

#### 6.2 Ranged Attack Execution
- **Files**: `Server/MirObjects/MonsterObject.cs`
- Projectile creation
- Range calculation
- Damage application
- Animation handling

#### 6.3 Ranged AI Behavior
- **Files**: `Server/MirObjects/Monsters/RangedMonster.cs` (NEW)
- Maintain distance from target
- Kite behavior
- Retreat when low HP
- Special ranged attack patterns

---

## 7. Skills System 💫

### Current State
- Spells enum exists
- Some spell implementations exist
- Need: Complete all spell implementations + UI

### Implementation Steps

#### 7.1 Complete All Spell Implementations
- **Files**: `Server/MirObjects/HumanObject.cs`
- Warrior Skills: Fencing, Slaying, Thrusting, HalfMoon, ShoulderDash, FlamingSword
- Wizard Skills: FireBall, Repulsion, ElectricShock, GreatFireBall, HellFire, ThunderBolt, Teleport, FireBang, FireWall, Lightning, FrostCrunch, ThunderStorm, MagicShield
- Taoist Skills: Healing, SpiritSword, Poisoning, SoulFireBall, SummonSkeleton, Hiding, MassHiding, SoulShield, Revelation, BlessedArmour, EnergyRepulsor, TrapHexagon, Purification, MassHealing, Hallucination, UltimateEnhancer

#### 7.2 Skill Requirements System
- **Files**: `Server/MirDatabase/MagicInfo.cs`
- Attribute requirements (replacing class requirements)
- Level requirements
- Previous skill requirements (skill trees)

#### 7.3 Skill Learning System
- **Files**: `Server/MirObjects/PlayerObject.cs`
- Learn from NPCs
- Learn from items (books)
- Skill point system
- Skill experience system

---

## 8. Skills UI 🎨

### Current State
- SkillDialog exists but is incomplete
- SkillBarDialog exists

### Implementation Steps

#### 8.1 Complete SkillDialog
- **Files**: `Client/MirScenes/Dialogs/SkillDialog.cs`
- Display all learned skills
- Show skill levels and experience
- Skill descriptions and requirements
- Skill upgrade interface

#### 8.2 Skill Tree Visualization
- **Files**: `Client/MirScenes/Dialogs/SkillDialog.cs`
- Visual skill tree
- Prerequisite display
- Available vs. learned skills

#### 8.3 Skill Bar Enhancement
- **Files**: `Client/MirScenes/Dialogs/MainDialogs.cs::SkillBarDialog`
- Multiple skill bars
- Skill cooldown display
- Skill keybind management

---

## 9. Player Inspect 👁️

### Implementation Steps

#### 9.1 Inspect Packet
- **Files**: `Shared/ClientPackets.cs`, `Shared/ServerPackets.cs`
- Add InspectPlayer packet
- Add InspectPlayerResponse packet
- Send equipped items and stats

#### 9.2 Inspect UI
- **Files**: `Client/MirScenes/Dialogs/InspectDialog.cs` (NEW)
- Display target player's equipment
- Show stats (read-only)
- Character model display
- Close button

#### 9.3 Inspect Command/Interaction
- **Files**: `Client/MirScenes/GameScene.cs`, `Server/MirObjects/PlayerObject.cs`
- Right-click inspect option
- Inspect command
- Range checking
- Permission checking

---

## 10. Quests UI ✅ EXISTS BUT NEEDS COMPLETION

### Current State
- QuestDialog exists
- Quest system partially implemented

### Implementation Steps

#### 10.1 Complete Quest UI
- **Files**: `Client/MirScenes/Dialogs/QuestDialogs.cs`
- Ensure all quest types display correctly
- Quest tracking UI
- Quest completion UI
- Quest sharing UI

#### 10.2 Quest Icons & Indicators
- **Files**: `Client/MirScenes/GameScene.cs`
- Quest icons above NPCs
- Quest completion indicators
- Quest item indicators

---

## 11. Quests Database 📚

### Implementation Steps

#### 11.1 Quest Data Structure
- **Files**: `Server/MirDatabase/QuestInfo.cs` (EXISTS)
- Complete all quest fields
- Quest chains
- Quest rewards
- Quest requirements

#### 11.2 Quest Creation Tools
- **Files**: `Server.MirForms/QuestBuilder/` (NEW)
- Quest editor form
- Quest chain editor
- Quest testing tools

#### 11.3 Quest Scripting System
- **Files**: `Server/MirObjects/NPC/NPCScript.cs`
- Quest acceptance scripts
- Quest completion scripts
- Quest update scripts
- Quest failure scripts

---

## 12. Caves Database 🕳️

### Implementation Steps

#### 12.1 Cave Map Data
- **Files**: `Server/MirDatabase/MapInfo.cs`
- Mark maps as caves
- Cave entry/exit points
- Cave depth/level system

#### 12.2 Cave Monster Placement
- **Files**: `Server/MirDatabase/RespawnInfo.cs`
- Cave-specific respawns
- Monster level scaling
- Cave boss placement

#### 12.3 Cave Requirements
- **Files**: `Server/MirDatabase/MapInfo.cs`
- Level requirements
- Good/Evil requirements
- Item requirements (keys, etc.)

---

## 13. Monster Stats Database 👹

### Implementation Steps

#### 13.1 Base Monster Stats
- **Files**: `Server/MirDatabase/MonsterInfo.cs`
- Complete all monster stat entries
- HP, AC, MAC, DC, MC, SC
- Attack speed, move speed
- Experience values

#### 13.2 Monster Stat Balancing
- **Files**: `Server.MirForms/Database/MonsterForm.cs`
- Stat editor interface
- Stat validation
- Stat scaling formulas

#### 13.3 Monster AI Assignment
- **Files**: `Server/MirDatabase/MonsterInfo.cs`
- Assign appropriate AI types
- Special abilities
- Attack patterns

---

## 14. Item Stats Database ⚔️

### Implementation Steps

#### 14.1 Complete Item Stats
- **Files**: `Shared/Data/ItemData.cs`
- All item stat entries
- Item grades and requirements
- Item set bonuses

#### 14.2 Item Stat Editor
- **Files**: `Server.MirForms/Database/ItemForm.cs`
- Complete item editor
- Stat validation
- Item preview

#### 14.3 Item Database Population
- **Files**: Database files
- Populate from archive data
- Verify stat accuracy
- Test item functionality

---

## Implementation Priority

### Phase 1: Foundation (Critical)
1. ✅ Magic Attack UI
2. 🔄 Attribute System (Replace Classes)
3. 🔄 Good/Evil System (Complete)

### Phase 2: Core Systems
4. Skills System (Complete implementations)
5. Skills UI (Complete)
6. Player Inspect
7. Quests UI (Complete)
8. Quests Database

### Phase 3: Advanced Features
9. Castle System
10. Archer Guard AI
11. TT System
12. Ranged AI

### Phase 4: Database Population
13. Caves Database
14. Monster Stats Database
15. Item Stats Database

---

## Technical Notes

### Attribute System Design
- Primary Attributes determine "class" role
- High Fire + High Magic = Wizard-like
- High Ground + High Attack = Warrior-like
- High Good/Evil + High Magic = Taoist-like
- Players can hybridize by distributing points

### Good/Evil System Design
- Independent leveling system
- Affects item/cave/NPC access
- Monsters give alignment-specific EXP
- Can switch alignment but with penalties

### Castle System Design
- Guild-based ownership
- Scheduled wars
- Economic benefits (taxes)
- Strategic importance

---

## Testing Checklist

For each implemented feature:
- [ ] Server-side functionality
- [ ] Client-side UI
- [ ] Network packets
- [ ] Database persistence
- [ ] Edge cases
- [ ] Performance
- [ ] Integration with existing systems

---

## Estimated Timeline

- Phase 1: 2-3 weeks
- Phase 2: 3-4 weeks
- Phase 3: 4-5 weeks
- Phase 4: 2-3 weeks

**Total: 11-15 weeks of focused development**

---

*Last Updated: [Current Date]*
*Status: In Progress*

