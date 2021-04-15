# vsmod-PatchDebug

This is a tool for mod developers.

Dumps JSON to logs before and after selected patches are made.

Rename your patch file to include `.debug.` in its filename to trigger dumping. For example, `assets/ropeorganization/patches/ropetricks.debug.json`.

Copious amounts of JSON will be dumped to your `server-debug.txt` and/or `client-debug.txt` log files.

Please make sure to rename your files back when you're done!

## Example

`assets/ropeorganization/patches/ropetricks.debug.json`:

```
[
  {
    "file": "game:itemtypes/resource/rope.json",
    "op": "add",
    "path": "/creativeinventory",
    "value": {
      "made_of_plants": ["*"],
      "long_and_thin": ["*"]
    },
  }
]
```

Excerpt from `server-debug.txt`:

```
15.4.2021 03:29:36 [VerboseDebug] === PatchDebug === patches/ropetricks.debug.json => game:itemtypes/resource/rope.json #0 === BEFORE ===
15.4.2021 03:29:36 [VerboseDebug] {
	code: "rope",
	texture: { base: "item/resource/rope" },
	creativeinventory: { "general": ["*"], "items": ["*"] },
	maxstacksize: 16,
	materialDensity: 510
}
15.4.2021 03:29:36 [VerboseDebug] === PatchDebug === patches/ropetricks.debug.json => game:itemtypes/resource/rope.json #0 === /BEFORE ===
15.4.2021 03:29:36 [VerboseDebug] === PatchDebug === patches/ropetricks.debug.json => game:itemtypes/resource/rope.json #0 === AFTER ===
15.4.2021 03:29:36 [VerboseDebug] {
  "code": "rope",
  "texture": {
    "base": "item/resource/rope"
  },
  "creativeinventory": {
    "made_of_plants": [
      "*"
    ],
    "long_and_thin": [
      "*"
    ]
  },
  "maxstacksize": 16,
  "materialDensity": 510
}
15.4.2021 03:29:36 [VerboseDebug] === PatchDebug === patches/ropetricks.debug.json => game:itemtypes/resource/rope.json #0 === /AFTER ===
```

Oops! `/creativeinventory` got replaced, deleting `"general": ["*"], "items": ["*"]`.

## Solution

The purpose of this mod is to help identify problems, but for posterity, here is the correct way to solve the problem above of adding multiple keys:

```
[
  {
    "file": "game:itemtypes/resource/rope.json",
    "op": "add",
    "path": "/creativeinventory/made_of_plants",
    "value": ["*"]
  },
  {
    "file": "game:itemtypes/resource/rope.json",
    "op": "add",
    "path": "/creativeinventory/long_and_thin",
    "value": ["*"]
  }
]
```
