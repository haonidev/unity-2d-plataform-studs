# Virtual Guy - Animation Design Document (ADD)

> Projeto de estudo: Platformer 2D inspirado em Hollow Knight, Celeste e
> Dead Cells.

------------------------------------------------------------------------

# 1. Personagem Base (Prompt Mestre)

## Objetivo

Recriar exatamente o mesmo personagem em futuras gerações mantendo
consistência visual.

### Prompt

``` text
Create a pixel art character called "Virtual Guy".

Style requirements:
- Match the visual style of Pixel Adventure 2.
- 32x32 pixels.
- Pixel perfect.
- No anti-aliasing.
- Transparent background.
- Dark blue tunic.
- Red scarf flowing behind the character.
- Brown leather belt.
- Dark boots.
- Brown messy hair.
- Light skin tone.
- Short katana sheathed on the back or waist.
- Same proportions, outline thickness, palette and shading style used in Pixel Adventure 2.
- Character must fit entirely inside a 32x32 frame.
```

------------------------------------------------------------------------

# 2. Regras Gerais para TODAS as animações

``` text
Requirements:
- Transparent background.
- Sprite sheet only.
- No text.
- No numbering.
- No shadows.
- No floor.
- Pixel perfect.
- No interpolation.
- No anti-aliasing.
- 32x32 pixels per frame.
- Horizontal sprite sheet.
- Keep exactly the same character design.
- Never change hair, palette, proportions or clothing.
```

------------------------------------------------------------------------

# 3. Idle

Frames: 8 Canvas: 256x32

Adicionar ao prompt:

``` text
Animation:
Idle

The character breathes naturally.
Small scarf movement.
Very subtle body movement.
Loop seamlessly.
```

------------------------------------------------------------------------

# 4. Running

Frames: 10 Canvas: 320x32

``` text
Animation:
Running

10 frames.

Fast platformer running cycle.

Alternate only contact and passing poses.

No exaggerated crouching.
No jumping pose.
No airborne pose.

Strong leg extension.

Natural arm swing.

Scarf follows movement.

Loop seamlessly.
```

------------------------------------------------------------------------

# 5. Jump

Frames: 4

``` text
Animation:
Jump

Takeoff only.

Includes:
Preparation
Push
Leave ground
Ascending
```

------------------------------------------------------------------------

# 6. Rising

Frames: 2

``` text
Animation:
Rising

Character ascending.

Legs slightly folded.

Scarf trailing downward.
```

------------------------------------------------------------------------

# 7. Falling

Frames: 2

``` text
Animation:
Falling

Body extended.

Legs slightly apart.

Scarf above the character due to falling.
```

------------------------------------------------------------------------

# 8. Landing

Frames: 3

``` text
Animation:
Landing

Soft landing.

Small squash.

Return to idle.
```

------------------------------------------------------------------------

# 9. Dash

Frames: 6

``` text
Animation:
Dash

Fast horizontal dash.

Aggressive forward lean.

Scarf stretched backwards.

Feet almost invisible due to speed.
```

------------------------------------------------------------------------

# 10. Ground Attack

Frames: 8

Canvas: 256x32

``` text
Animation:
Ground Attack

Short katana.

Frame sequence:

1 Idle
2 Anticipation
3 Slash begins
4 Mid slash
5 Impact
6 Follow through
7 Recovery
8 Idle

Fast attack.

No exaggerated movement.

Feet remain grounded.
```

------------------------------------------------------------------------

# 11. Air Attack

Frames: 7

``` text
Animation:
Air Attack

Horizontal slash while airborne.

Legs tucked.

Maintain aerial momentum.
```

------------------------------------------------------------------------

# 12. Hurt

Frames: 4

``` text
Animation:
Hurt

Quick hit reaction.

Small backward movement.

No dramatic knockback.
```

------------------------------------------------------------------------

# 13. Death

Frames: 8

``` text
Animation:
Death

Collapse.

Lose balance.

Disappear at the end.
```

------------------------------------------------------------------------

# Convenções

  Item            Valor
  --------------- --------------------------------------
  Frame           32x32 px
  Fundo           Transparente
  Layout          Horizontal
  Anti-aliasing   Não
  Pixel Perfect   Sim
  Escala          1:1
  Outline         Igual ao Pixel Adventure 2
  Paleta          Igual ao Pixel Adventure 2
  Personagem      Sempre idêntico
  Katana          Curta
  Estilo          Hollow Knight + Celeste + Dead Cells
