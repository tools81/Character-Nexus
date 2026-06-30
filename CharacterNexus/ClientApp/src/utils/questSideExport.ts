// Converts a CharacterNexus character object to a QuestSide-compatible import JSON.

const RULESET_TO_SYSTEM: Record<string, string> = {
  AmazingTales:         'amazingTales',
  BladeRunner:          'bladeRunner',
  CallOfCthulhu:        'coc',
  DarkCrystal:          'darkCrystal',
  EverydayHeroes:       'everydayHeroes',
  Fallout:              'fallout',
  FinalFantasy:         'ffxiv',
  Ghostbusters:         'ghostbusters',
  Marvel:               'marvelMultiverse',
  TMNT:                 'tmnt',
  Transformers:         'transformers',
  VampireTheMasquerade: 'vtm5e',
  WorldWideWrestling:   'worldWideWrestling',
};

// react-hook-form stores array items as [{value: "X"}]; extract the strings.
function vals(arr: unknown): string[] {
  if (!Array.isArray(arr)) return [];
  return (arr as any[])
    .map(item => (item && typeof item === 'object' ? String(item.value ?? '') : String(item ?? '')))
    .filter(Boolean);
}

function toText(arr: unknown): string {
  return vals(arr).join('\n');
}

// BladeRunner YZE: integer score → A/B/C/D rank letter
function numToAbcd(n: number): string {
  if (n >= 7) return 'A';
  if (n >= 4) return 'B';
  if (n >= 1) return 'C';
  return 'D';
}

// BladeRunner YZE: 1–4 skill rank → A/B/C/D
function rankToAbcd(n: number): string {
  if (n >= 4) return 'A';
  if (n >= 3) return 'B';
  if (n >= 2) return 'C';
  return 'D';
}

// ── Mappers ───────────────────────────────────────────────────────────────────

function mapAmazingTales(cn: any): object {
  return {
    name: cn.name || '',
    stats: { stat1: 'd12', stat2: 'd10', stat3: 'd8', stat4: 'd6' },
    statLabels: {
      stat1: cn.d12Attribute || 'Stat 1',
      stat2: cn.d10Attribute || 'Stat 2',
      stat3: cn.d8Attribute  || 'Stat 3',
      stat4: cn.d6Attribute  || 'Stat 4',
    },
    hp: { current: 3, max: 3 },
    notes: cn.notes || '',
  };
}

function mapBladeRunner(cn: any): object {
  const mem = cn.memory || {};
  const rel = cn.relationship || {};
  return {
    name:          cn.name || '',
    characterType: cn.origin || 'Human',
    archetype:     cn.archetype || '',
    yearsOnForce:  cn.tenure || '',
    badgeNumber:   '',
    stats: {
      strength:     numToAbcd(Number(cn.attributes?.Strength)     || 1),
      agility:      numToAbcd(Number(cn.attributes?.Agility)      || 1),
      intelligence: numToAbcd(Number(cn.attributes?.Intelligence) || 1),
      empathy:      numToAbcd(Number(cn.attributes?.Empathy)      || 1),
    },
    skills: {
      force:        rankToAbcd(Number(cn.skills?.Force)            || 1),
      handToHand:   rankToAbcd(Number(cn.skills?.['Close Combat']) || 1),
      stamina:      rankToAbcd(Number(cn.skills?.Stamina)          || 1),
      mobility:     rankToAbcd(Number(cn.skills?.Mobility)         || 1),
      stealth:      rankToAbcd(Number(cn.skills?.Stealth)          || 1),
      firearms:     rankToAbcd(Number(cn.skills?.Firearms)         || 1),
      observation:  rankToAbcd(Number(cn.skills?.Observation)      || 1),
      tech:         rankToAbcd(Number(cn.skills?.Tech)             || 1),
      medicalAid:   rankToAbcd(Number(cn.skills?.['Medical Aid'])  || 1),
      connections:  rankToAbcd(Number(cn.skills?.Connections)      || 1),
      insight:      rankToAbcd(Number(cn.skills?.Insight)          || 1),
      manipulation: rankToAbcd(Number(cn.skills?.Manipulation)     || 1),
      driving:      rankToAbcd(Number(cn.skills?.Driving)          || 1),
    },
    resources: {
      damage:          { current: 0, max: 4 },
      stress:          { current: 0, max: 4 },
      promotionPoints: { current: Number(cn.promotionpoints) || 0, max: 10 },
    },
    keyMemory:       [mem.when, mem.who, mem.where, mem.what, mem.how].filter(Boolean).join(' · '),
    keyRelationship: [rel.who, rel.what, rel.status].filter(Boolean).join(' · '),
    appearance:      cn.appearance || '',
    hangUp: [
      cn.home         ? `Home: ${cn.home}`                 : '',
      cn.signatureitem ? `Signature Item: ${cn.signatureitem}` : '',
    ].filter(Boolean).join('\n'),
    specialties:      toText(cn.specialties),
    criticalInjuries: '',
    weapons:          toText(cn.weapons),
    chinyenPoints:    String(cn.chinyen ?? 0),
    humanityPoints:   String(cn.humanitypoints ?? 0),
    standardGear:     toText(cn.gears),
    personalGear: [
      ...vals(cn.augmentations).map(a => `[Augmentation] ${a}`),
      ...vals(cn.armors).map(a         => `[Armor] ${a}`),
      ...vals(cn.vehicles).map(v       => `[Vehicle] ${v}`),
    ].join('\n'),
    notes: cn.notes || '',
  };
}

const COC_SKILLS: Record<string, string> = {
  'Accounting': 'accounting', 'Acting': 'acting', 'Animal Handling': 'animalHandling',
  'Anthropology': 'anthropology', 'Appraise': 'appraise', 'Archaeology': 'archaeology',
  'Art and Craft': 'artCraft', 'Artillery': 'artillery', 'Astronomy': 'astronomy',
  'Axe': 'axe', 'Biology': 'biology', 'Botany': 'botany', 'Bow': 'bow',
  'Brawl': 'brawl', 'Chainsaw': 'chainsaw', 'Charm': 'charm', 'Chemistry': 'chemistry',
  'Climb': 'climb', 'Computer Use': 'computerUse', 'Credit Rating': 'creditRating',
  'Cryptography': 'cryptography', 'Cthulhu Mythos': 'cthulhuMythos',
  'Demolitions': 'demolitions', 'Disguise': 'disguise', 'Diving': 'diving',
  'Dodge': 'dodge', 'Drive Auto': 'driveAuto', 'Electrical Repair': 'electricalRepair',
  'Electronics': 'electronics', 'Engineering': 'engineering', 'Fast Talk': 'fastTalk',
  'Fighting': 'fighting', 'Fine Art': 'fineArt', 'Firearms': 'firearms',
  'First Aid': 'firstAid', 'Flail': 'flail', 'Flamethrower': 'flamethrower',
  'Forensics': 'forensics', 'Forgery': 'forgery', 'Garrote': 'garrote',
  'Geology': 'geology', 'Handgun': 'handgun', 'Heavy Weapons': 'heavyWeapons',
  'History': 'history', 'Hypnosis': 'hypnosis', 'Intimidate': 'intimidate',
  'Jump': 'jump', 'Language (Other)': 'languageOther', 'Language (Own)': 'languageOwn',
  'Law': 'law', 'Library Use': 'libraryUse', 'Listen': 'listen', 'Locksmith': 'locksmith',
  'Lore': 'lore', 'Machine Gun': 'machineGun', 'Mathematics': 'mathematics',
  'Mechanical Repair': 'mechanicalRepair', 'Medicine': 'medicine',
  'Meteorology': 'meteorology', 'Natural World': 'naturalWorld', 'Navigate': 'navigate',
  'Occult': 'occult', 'Operate Heavy Machinery': 'operateHeavy', 'Persuade': 'persuade',
  'Pharmacy': 'pharmacy', 'Photography': 'photography', 'Physics': 'physics',
  'Pilot': 'pilot', 'Psychoanalysis': 'psychoanalysis', 'Psychology': 'psychology',
  'Read Lips': 'readLips', 'Ride': 'ride', 'Rifle/Shotgun': 'rifleShotgun',
  'Science': 'science', 'Shotgun': 'shotgun', 'Sleight of Hand': 'sleightOfHand',
  'Spear': 'spear', 'Spot Hidden': 'spotHidden', 'Stealth': 'stealth',
  'Submachine Gun': 'submachineGun', 'Survival': 'survival', 'Swim': 'swim',
  'Sword': 'sword', 'Throw': 'throw', 'Track': 'track', 'Whip': 'whip',
  'Zoology': 'zoology',
};

function mapCallOfCthulhu(cn: any): object {
  const charMap: Record<string, string> = {
    Strength: 'str', Constitution: 'con', Size: 'siz', Dexterity: 'dex',
    Appearance: 'app', Intelligence: 'int', Power: 'pow', Education: 'edu',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(charMap)) {
    if (cn.characteristics?.[k] != null) stats[v] = Number(cn.characteristics[k]);
  }

  const skills: Record<string, number> = {};
  if (cn.skills) {
    for (const [k, v] of Object.entries(cn.skills as Record<string, unknown>)) {
      const qsKey = COC_SKILLS[k];
      if (qsKey && v != null) skills[qsKey] = Number(v);
    }
  }

  const hp     = Number(cn.hitPoints)   || 0;
  const sanity = Number(cn.sanityPoints) || 50;
  const magic  = Number(cn.magicPoints)  || 10;
  const luck   = Number(cn.characteristics?.Luck) || 50;

  return {
    name: cn.name || '', occupation: cn.occupation || '',
    age: String(cn.age || ''), residence: cn.residence || '', birthplace: cn.birthplace || '',
    stats, skills,
    hp: { current: hp, max: hp },
    resources: {
      sanity:   { current: sanity, max: sanity },
      luck,
      magicPts: { current: magic, max: magic },
    },
    combatFields: {
      db:       cn.damageBonus || '0',
      build:    Number(cn.build)    || 0,
      moveRate: Number(cn.movement) || 8,
    },
    backstory:           cn.description        || '',
    ideology:            cn.ideology           || '',
    significantPeople:   cn.significantPeople  || '',
    meaningfulLocations: cn.meaningfulLocations || '',
    treasures:           cn.treasuredPossessions || '',
    traits:              cn.traits             || '',
    weapons:   toText(cn.weapons),
    equipment: toText(cn.equipments),
    spells:    toText(cn.spells),
    injuries:  [toText(cn.phobias), toText(cn.manias)].filter(Boolean).join('\n'),
    notes:     cn.notes || '',
  };
}

function mapDarkCrystal(cn: any): object {
  const SKILL_KEYS = ['agility', 'animals', 'fighting', 'lore', 'brawn', 'scouting', 'social'];
  const skills: Record<string, { level: number; specialization: string }> = {};
  for (const key of SKILL_KEYS) {
    const label = key.charAt(0).toUpperCase() + key.slice(1);
    const trained = !!(cn.skills?.[key] ?? cn.skills?.[label]);
    const specArr = cn.specializations?.[label];
    skills[key] = { level: trained ? 1 : 0, specialization: vals(specArr)[0] || '' };
  }
  return {
    name: cn.name || '',
    combatFields: { clan: cn.clan || '', wings: cn.gender === 'Female' ? 'Yes' : 'No' },
    resources: { creatureDie: 'd6' },
    skills,
    clanTraits:   toText(cn.traits),
    flaw:         toText(cn.flaws),
    summons:      '',
    dreamfasting: '',
    description:  cn.notes || '',
    traits:       toText(cn.traits),
    injuries:     '',
    xp:           '',
    gear:         toText(cn.gears),
    heavyGear:    '',
    notes:        cn.notes || '',
  };
}

function mapEverydayHeroes(cn: any): object {
  const attrMap: Record<string, string> = {
    Strength: 'str', Dexterity: 'dex', Constitution: 'con',
    Intelligence: 'int', Wisdom: 'wis', Charisma: 'cha',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(attrMap)) {
    if (cn.attribute?.[k] != null) stats[v] = Number(cn.attribute[k]);
  }

  const skills: Record<string, number> = {};
  if (cn.skills?.proficient) {
    for (const [k, v] of Object.entries(cn.skills.proficient as Record<string, unknown>)) {
      if (v) skills[k.charAt(0).toLowerCase() + k.slice(1)] = 1;
    }
  }
  if (cn.skills?.expertise) {
    for (const [k, v] of Object.entries(cn.skills.expertise as Record<string, unknown>)) {
      if (v) skills[k.charAt(0).toLowerCase() + k.slice(1)] = 2;
    }
  }

  const hp = Number(cn.hitpoints) || 0;
  return {
    name: cn.name || '', level: Number(cn.level) || 1,
    background: cn.background || '', profession: cn.profession || '', archetype: cn.archetype || '',
    stats, skills,
    hp: { current: hp, max: hp },
    notes: cn.notes || '',
  };
}

function mapFallout(cn: any): object {
  const attrMap: Record<string, string> = {
    Strength: 'str', Perception: 'per', Endurance: 'end',
    Charisma: 'cha', Intelligence: 'int', Agility: 'agi', Luck: 'lck',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(attrMap)) {
    if (cn.attributes?.[k] != null) stats[v] = Number(cn.attributes[k]);
  }

  const skills: Record<string, number> = {};
  if (cn.skills) {
    for (const [k, v] of Object.entries(cn.skills as Record<string, unknown>)) {
      const key = k.charAt(0).toLowerCase() + k.slice(1);
      if (v != null) skills[key] = Number(v);
    }
  }

  const hp = Number(cn.healthPoints) || 0;
  return {
    name: cn.name || '', level: Number(cn.level) || 1,
    origin: cn.origin || '', faction: cn.faction || '',
    stats, skills,
    hp: { current: hp, max: hp },
    caps:    Number(cn.caps) || 0,
    traits:  toText(cn.traits),
    perks:   toText(cn.perks),
    weapons: toText(cn.weapons),
    armor:   toText(cn.armors),
    notes:   cn.notes || '',
  };
}

function mapFinalFantasy(cn: any): object {
  const attrMap: Record<string, string> = {
    Strength: 'str', Dexterity: 'dex', Vitality: 'vit', Intelligence: 'int', Mind: 'mnd',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(attrMap)) {
    if (cn.attributes?.[k] != null) stats[v] = Number(cn.attributes[k]);
  }

  const hp = Number(cn.hp) || 0;
  const mp = Number(cn.mp) || 0;
  return {
    name:  cn.name || '',
    job:   cn.job  || '',
    level: Number(cn.level) || 1,
    race:  cn.race || '',
    stats,
    hp: { current: hp, max: hp },
    mp: { current: mp, max: mp },
    jobInfo: [
      cn.role       ? `Role: ${cn.role}`             : '',
      cn.limitBreak ? `Limit Break: ${cn.limitBreak}` : '',
      cn.augmentation ? `Augmentation: ${cn.augmentation}` : '',
    ].filter(Boolean).join('\n'),
    profileTraits: cn.profile || '',
    traits:        toText(cn.traits),
    companions:    '',
    items:         toText(cn.items),
    augmentations: toText(cn.abilities),
    combatFields: {
      defense:      Number(cn.defense)      || 0,
      magicDefense: Number(cn.magicDefense) || 0,
      vigilance:    Number(cn.vigilance)    || 0,
      speed:        Number(cn.speed)        || 0,
    },
    notes: cn.notes || '',
  };
}

function mapGhostbusters(cn: any): object {
  const KEYS = ['Brains', 'Muscle', 'Moves', 'Cool'];
  const QS_KEYS: Record<string, string> = { Brains: 'brains', Muscle: 'muscles', Moves: 'moves', Cool: 'cool' };
  const stats: Record<string, number> = {};
  const talents: Record<string, string> = {};
  for (const k of KEYS) {
    stats[QS_KEYS[k]]   = Number(cn.traits?.[k])  || 1;
    talents[QS_KEYS[k]] = cn.talents?.[k] || '';
  }
  return {
    name:          cn.name        || '',
    alias:         cn.alias       || '',
    goal:          cn.goal        || '',
    description:   cn.description || '',
    stats, talents,
    hp: { current: 0, max: 0 },
    browniePoints: Number(cn.browniePoints) || 0,
    weapons:       toText(cn.weapons),
    gear:          toText(cn.gears),
    notes:         cn.notes || '',
  };
}

function mapMarvel(cn: any): object {
  const attrMap: Record<string, string> = {
    Melee: 'melee', Agility: 'agility', Resilience: 'resilience',
    Vigilance: 'vigilance', Ego: 'ego', Logic: 'logic',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(attrMap)) {
    if (cn.attributes?.[k] != null) stats[v] = Number(cn.attributes[k]);
  }

  const hp = Number(cn.health) || 0;
  return {
    name: cn.name || '', realName: cn.realName || '',
    origin: cn.origin || '', occupation: cn.occupation || '', rank: cn.rank || '',
    stats,
    hp: { current: hp, max: hp },
    resources: {
      focus: { current: Number(cn.focus) || 0, max: Number(cn.focus) || 10 },
      karma: { current: Number(cn.karma) || 0, max: 10 },
    },
    traits:      toText(cn.trait),
    tags:        toText(cn.tag),
    weapons:     toText(cn.weapon),
    history:     cn.history     || '',
    personality: cn.personality || '',
    notes:       cn.notes       || '',
  };
}

function mapTMNT(cn: any): object {
  const attrMap: Record<string, string> = {
    IQ: 'iq', ME: 'me', MA: 'ma', PS: 'ps', PP: 'pp', PE: 'pe', PB: 'pb', Spd: 'spd',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(attrMap)) {
    if (cn.attributes?.[k] != null) stats[v] = Number(cn.attributes[k]);
  }

  const skillLines: string[] = [];
  if (cn.skills && typeof cn.skills === 'object') {
    for (const [k, v] of Object.entries(cn.skills as Record<string, unknown>)) {
      if (v != null && v !== '' && Number(v) !== 0) skillLines.push(`${k}: ${v}%`);
    }
  }

  const hp = Number(cn.hitPoints) || 0;
  return {
    name: cn.name || '',
    stats,
    hp: { current: hp, max: hp },
    mutant: [
      cn.animal    ? `Animal: ${cn.animal}`       : '',
      cn.mutation  ? `Mutation: ${cn.mutation}`   : '',
      cn.education ? `Education: ${cn.education}` : '',
      `XP: ${cn.xp || 0}  Level: ${cn.level || 1}`,
    ].filter(Boolean).join('\n'),
    background: [
      cn.alignment    ? `Alignment: ${cn.alignment}`       : '',
      cn.organization ? `Organization: ${cn.organization}` : '',
      cn.origin       ? `Origin: ${cn.origin}`             : '',
    ].filter(Boolean).join('\n'),
    appearance: [
      cn.gender ? `Gender: ${cn.gender}` : '',
      cn.age    ? `Age: ${cn.age}`        : '',
      cn.height ? `Height: ${cn.height}`  : '',
      cn.weight ? `Weight: ${cn.weight}`  : '',
      cn.size   ? `Size Level: ${cn.size}` : '',
      `SDC: ${cn.sdc || 0}`,
    ].filter(Boolean).join('\n'),
    handToHand:     skillLines.filter(l => /Hand|Combat|Boxing|Wrestling|Martial|Acrobatic|Gymnastic|Boxing|Fencing/i.test(l)).join('\n'),
    weapons:        toText(cn.weapons),
    naturalWeapons: '',
    equipment: [
      toText(cn.equipments),
      cn.vehicle  ? `Vehicle: ${cn.vehicle}` : '',
      cn.armor    ? `Armor: ${cn.armor}`      : '',
      toText(cn.psionics) ? `Psionics:\n${toText(cn.psionics)}` : '',
    ].filter(Boolean).join('\n'),
    notes: [cn.notes || '', skillLines.length ? `Skills:\n${skillLines.join('\n')}` : ''].filter(Boolean).join('\n\n'),
  };
}

function mapTransformers(cn: any): object {
  const essenceMap: Record<string, string> = {
    Strength: 'strength', Speed: 'speed', Smarts: 'smarts', Social: 'social',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(essenceMap)) {
    if (cn.essences?.[k] != null) stats[v] = Number(cn.essences[k]);
  }

  const skills: Record<string, number> = {};
  const skillSpecs: Record<string, string> = {};
  if (cn.skills) {
    for (const [k, v] of Object.entries(cn.skills as Record<string, unknown>)) {
      const key = k.replace(/\s+(.)/g, (_, c: string) => c.toUpperCase()).replace(/^./, c => c.toLowerCase());
      skills[key] = Number(v) || 0;
    }
  }
  if (cn.specialization) {
    for (const [k, v] of Object.entries(cn.specialization as Record<string, unknown>)) {
      const key = k.replace(/\s+(.)/g, (_, c: string) => c.toUpperCase()).replace(/^./, c => c.toLowerCase());
      if (v) skillSpecs[key] = String(v);
    }
  }

  const focusParts: string[] = [];
  if (cn.focus) {
    for (const [k, v] of Object.entries(cn.focus as Record<string, unknown>)) {
      if (v) focusParts.push(k.charAt(0).toUpperCase() + k.slice(1));
    }
  }

  return {
    name: cn.name || '', level: Number(cn.level) || 1,
    faction: cn.faction || '', origin: cn.origin || '', role: cn.role || '',
    stats, skills, skillSpecs,
    hp: { current: 0, max: 0 },
    perks:       toText(cn.perks),
    hangups:     toText(cn.hangups),
    influences:  toText(cn.influences),
    focus:       focusParts.join(', '),
    description: cn.description || '',
    notes:       cn.notes       || '',
  };
}

function mapVampireTheMasquerade(cn: any): object {
  const attrMap: Record<string, string> = {
    Strength: 'strength', Dexterity: 'dexterity', Stamina: 'stamina',
    Charisma: 'charisma', Manipulation: 'manipulation', Composure: 'composure',
    Intelligence: 'intelligence', Wits: 'wits', Resolve: 'resolve',
  };
  const stats: Record<string, number> = {};
  for (const [k, v] of Object.entries(attrMap)) {
    if (cn.attributes?.[k] != null) stats[v] = Number(cn.attributes[k]);
  }

  const skillMap: Record<string, string> = {
    Athletics: 'athletics', Brawl: 'brawl', Craft: 'craft', Drive: 'drive',
    Firearms: 'firearms', Larceny: 'larceny', Melee: 'melee', Stealth: 'stealth',
    Survival: 'survival', 'Animal Ken': 'animalKen', Etiquette: 'etiquette',
    Insight: 'insight', Intimidation: 'intimidation', Leadership: 'leadership',
    Performance: 'performance', Persuasion: 'persuasion', Streetwise: 'streetwise',
    Subterfuge: 'subterfuge', Academics: 'academics', Awareness: 'awareness',
    Finance: 'finance', Investigation: 'investigation', Medicine: 'medicine',
    Occult: 'occult', Politics: 'politics', Science: 'science', Technology: 'technology',
  };
  const skills: Record<string, number> = {};
  for (const [k, v] of Object.entries(skillMap)) {
    if (cn.skills?.[k] != null) skills[v] = Number(cn.skills[k]);
  }

  const healthVal    = Number(cn.health)    || 6;
  const willpowerVal = Number(cn.willpower) || 4;

  return {
    name: cn.name || '', concept: cn.concept || '',
    clan: cn.clan || '', predator: cn.predator || '', generation: cn.generation || '',
    sire: cn.sire || '', ambition: cn.ambition || '', desire: cn.desire || '',
    chronicleTenets: cn.chronicleTenets || '',
    touchstonesConvictions: cn.touchstonesConvictions || '',
    age: cn.age || '',
    stats, skills,
    resources: {
      health:       { boxes: new Array(healthVal).fill(false) },
      willpower:    { boxes: new Array(willpowerVal).fill(false) },
      humanity:     Number(cn.humanity)     || 7,
      bloodPotency: Number(cn.bloodPotency) || 1,
      hunger:       Number(cn.hunger)       || 1,
    },
    history:               cn.history               || '',
    appearance:            cn.appearance            || '',
    distinguishingFeatures: cn.distinguishingFeatures || '',
    rituals:               toText(cn.rituals),
    weapons:               toText(cn.weapons),
    armor:                 toText(cn.armors),
    gear:                  toText(cn.gears),
    notes:                 cn.notes                 || '',
    totalExperience:       Number(cn.totalExperience) || 0,
    spentExperience:       Number(cn.spentExperience) || 0,
  };
}

function mapWorldWideWrestling(cn: any): object {
  const stats: Record<string, number> = {
    power: Number(cn.stats?.Body) || 0,
    look:  Number(cn.stats?.Look) || 0,
    real:  Number(cn.stats?.Real) || 0,
    work:  Number(cn.stats?.Work) || 0,
  };

  const role = cn.role || '';
  const qSection = cn.questions?.[role] || {};
  const questionsText = Object.entries(qSection as Record<string, unknown>)
    .map(([q, a]) => `Q: ${q}\nA: ${a}`)
    .join('\n\n');

  return {
    name: cn.name || '',
    role,
    gimmick: [
      cn.gimmick          ? `Gimmick: ${cn.gimmick}`                     : '',
      cn.hailing?.[role]  ? `Hailing From: ${cn.hailing[role]}`          : '',
      cn.entrance?.[role] ? `Entrance: ${cn.entrance[role]}`             : '',
    ].filter(Boolean).join('\n'),
    concept:     questionsText,
    stats,
    want:        toText(cn.wants),
    gimmickMoves: toText(cn.moves),
    notes:       cn.notes || '',
  };
}

// ── Dispatch ──────────────────────────────────────────────────────────────────

const MAPPERS: Record<string, (cn: any) => object> = {
  AmazingTales:         mapAmazingTales,
  BladeRunner:          mapBladeRunner,
  CallOfCthulhu:        mapCallOfCthulhu,
  DarkCrystal:          mapDarkCrystal,
  EverydayHeroes:       mapEverydayHeroes,
  Fallout:              mapFallout,
  FinalFantasy:         mapFinalFantasy,
  Ghostbusters:         mapGhostbusters,
  Marvel:               mapMarvel,
  TMNT:                 mapTMNT,
  Transformers:         mapTransformers,
  VampireTheMasquerade: mapVampireTheMasquerade,
  WorldWideWrestling:   mapWorldWideWrestling,
};

/**
 * Convert a CharacterNexus character to a QuestSide-compatible JSON object.
 * Returns null if the ruleset has no QuestSide mapping.
 */
export function exportToQuestSide(
  rulesetName: string,
  character: any,
): { systemId: string; data: object } | null {
  const systemId = RULESET_TO_SYSTEM[rulesetName];
  const mapper   = MAPPERS[rulesetName];
  if (!systemId || !mapper) return null;
  return { systemId, data: mapper(character) };
}

export { RULESET_TO_SYSTEM };
