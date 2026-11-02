# Apollon Volados — Design Brief

## 1. Category and competitive context

A **community football club app**: news, teams, history and contact for a 100-year-old village club. The benchmarks:

| App | What it gets right |
| --- | --- |
| **Liverpool FC (official)** | The club identity leads: crest, club colour and heritage. News is editorial, with one big lead story and a calm list below. |
| **OneFootball** | Dense but readable lists, very consistent card anatomy, true dark mode, fast scanning. |
| **Apple Sports** | Restraint. Neutral surfaces, team colour used only as an accent, system type, native navigation, no chrome for its own sake. |

Common ground: neutral surfaces with the brand colour used **sparingly**, one strong editorial lead, native navigation, and a crest treated as a precious object, never as wallpaper.

## 2. Target user and emotional tone

- **Who:** villagers of Volada (Karpathos), the diaspora in Athens and the USA (the club has branches in both), academy parents and older members. Many users are 50+ and Greek-first.
- **Context:** a quick glance on a phone, often from abroad, to feel connected to home. Low frequency, high emotional value.
- **Tone:** **heritage, warm, trustworthy**. Proud but not loud. It should feel like a well-kept clubhouse with the trophy cabinet and the old photos, not a betting app. Calm over flashy, legible over clever.

## 3. Visual direction: "Heritage Editorial"

The palette and ornament come straight from the crest: **crest green**, **bronze lettering**, the **parchment** disc and **ink** black. Serif display type evokes classical Greece and the 1924 date; system sans keeps the body text crisp and readable. No gradients, no glassy decoration of our own, no emoji as primary iconography inside headers.

### Colour system (`Resources/Styles/Colors.xaml`)

| Role | Light | Dark | Notes |
| --- | --- | --- | --- |
| Primary (crest green) | `#007A3D` | `#4CC285` | Buttons, active tab, links. 5.4:1 on white |
| Primary strong | `#0B4F2C` | `#8FDDB2` | Pressed state, headlines on tinted areas |
| Primary container | `#E3F1E8` | `#13392A` | Icon badges, selected rows |
| On primary | `#FFFFFF` | `#04210F` | Text and icons on Primary |
| Secondary (bronze) | `#8A6324` | `#D9B36C` | Years, overlines, the signature ring |
| Secondary hairline | `#C9A24A` | `#8C7240` | Decorative rules only, never text |
| Background | `#FAF7F0` (parchment) | `#0E1612` (pine night) | Page background |
| Surface | `#FFFFFF` | `#16211B` | Cards |
| Surface sunken | `#F2EEE4` | `#1E2B24` | Skeletons, image wells, input fields |
| Outline | `#E7E1D3` | `#2A3830` | Card borders, dividers |
| Text primary (ink) | `#1B1F1C` | `#F2EFE6` | |
| Text secondary | `#5B625D` | `#A9B3AC` | Meta, descriptions |
| Text tertiary | `#8A8F8A` | `#76807A` | Disabled, placeholders |
| Error | `#B3261E` | `#F2B8B5` | |
| Warning | `#8F5A00` | `#F3C26B` | |
| Success | = Primary | = Primary | |

### Type scale (`Styles.xaml`, `Text.*` styles)

Display text uses the **platform serif** (Georgia on iOS, Noto Serif via `serif` on Android). Both have full Greek coverage. The previously registered Playfair and Baskerville files were never bundled and don't include Greek. Everything else uses the **system sans** (SF Pro / Roboto) so Dynamic Type and Greek rendering are native.

| Style | Font | Size | Weight | Line height |
| --- | --- | --- | --- | --- |
| `Text.Display` | Serif | 34 | Bold | 1.1 |
| `Text.Title1` | Serif | 28 | Bold | 1.15 |
| `Text.Title2` | Serif | 22 | Bold | 1.2 |
| `Text.Title3` | Sans | 18 | Bold | 1.25 |
| `Text.Headline` | Sans | 16 | Bold | 1.3 |
| `Text.Body` | Sans | 16 | Regular | 1.4 |
| `Text.Callout` | Sans | 15 | Regular | 1.4 |
| `Text.Subhead` | Sans | 14 | Regular | 1.35 |
| `Text.Footnote` | Sans | 13 | Regular | 1.3 |
| `Text.Caption` | Sans | 12 | Regular | 1.3 |
| `Text.Overline` | Sans | 12 | Bold, +1.5 tracking, bronze, written in capitals **without accents** (Greek all-caps drops the tonos, so we don't use `TextTransform`) | 1.2 |
| `Text.Year` | Serif | 20 | Bold, bronze | 1.0 |

### Spacing scale (4-pt grid)

`Space1`=4, `Space2`=8, `Space3`=12, `Space4`=16, `Space5`=20, `Space6`=24, `Space8`=32, `Space12`=48. Page gutters are 20; gaps between cards are 12; gaps between sections are 32.

### Corner radius

| Token | Value | Used for |
| --- | --- | --- |
| XS | 6 | progress bars, small tags |
| S | 10 | thumbnails, image wells |
| M | 16 | cards, buttons |
| L | 24 | the lead story card |
| Full | 50% | crest, icon badges, timeline nodes |

### Elevation

- **Level 0 (default):** flat on the background.
- **Level 1 (cards):** a 1-px `Outline` border, no shadow. Parchment plus a hairline reads as paper, so we don't need shadows.
- **Level 2 (floating):** a soft shadow (`#1A000000`, y = 6, blur 18), **only** on the lead story card and the crest on the splash screen.
- **Dark mode:** no shadows. Elevation is shown by moving from `Surface` to `Surface sunken`.
- **Android:** no shadows either. MAUI draws a `Border` shadow there as a rectangle that ignores the rounded corners, so Android cards rely on the Level 1 hairline (like Material's outlined cards).

## 4. Platform-appropriate patterns

**Shared across platforms (the .NET MAUI design system):** colour tokens, type scale, spacing, radius, card anatomy, the crest ring, the content hierarchy, and the four-tab information architecture (Αρχική, Νέα, Τμήματα, Σύλλογος).

| Concern | iOS | Android |
| --- | --- | --- |
| Primary navigation | System tab bar (Liquid Glass on iOS 26+). We set the tint only; the system draws the bar. | Material bottom navigation on `Surface`, with green active colour. |
| Back from pushed pages | `‹ Πίσω` text button at top-left, plus the system edge-swipe gesture | `←` icon button, plus the system/predictive back gesture |
| Feedback on tap | Light selection haptic on list taps (`HapticFeedback.Click`) | Ripple only (no extra `VIBRATE` permission) |
| Safe areas | Content respects the Dynamic Island and home indicator; no hard-coded margins | Edge-to-edge; the system pads the status and navigation bars |
| System bar icons | Handled by iOS | Dark icons on parchment, light icons on pine night (`MainActivity`) |
| Text scaling | Dynamic Type through `FontAutoScalingEnabled` (on by default) | Font-scale setting, same mechanism |
| Light / dark | Follows the system by default; the Club tab's *Εμφάνιση* switch can force Light or Dark (saved per device) | Same |

## 5. Signature detail: the Crest Ring

The crest is framed by **a green ring with a thin bronze ring inside it**. The app reuses that one motif wherever it needs an accent:

- **Section and page titles** sit on a *crest rule*: a short, 3-px green bar with a bronze hairline that runs to the edge.
- **Timeline nodes** on the Club tab are green discs with a bronze ring, threaded on a green spine. They read like the crest's ring, unrolled into history.
- **Team badges** are green discs ringed in bronze.
- **The crest** on the splash and home screens sits inside a bronze hairline ring.

It comes from the club's own badge, so it identifies the app without adding decoration. It is cheap to draw (two `BoxView`s or `Border`s) and it holds up in dark mode.
