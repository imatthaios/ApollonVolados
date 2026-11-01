# Publishing Apollon Volados to the App Store and Google Play

A step-by-step guide for **Α.Σ. Απόλλων Βωλάδος**, a non-profit sports club (σωματείο), to publish this .NET MAUI app under the **club's own name** on Apple's App Store and Google Play.

> Store rules change often. Everything below was checked on **1 November 2026**; the sources are at the end. Re-check anything marked ⚠️ before you rely on it.

---

## 0. Overview

| | Apple App Store | Google Play |
| --- | --- | --- |
| Account type | **Organization** (not Individual) | **Organization** (not Personal) |
| Cost | 99 USD/year; non-profits can request a **fee waiver** (§2.2) | 25 USD one-time |
| Needs a D-U-N-S number | Yes | Yes |
| Typical time to first release | 2–4 weeks (D-U-N-S + enrollment + review) | 2–5 weeks (D-U-N-S + verification + review) |
| Build tool requirement | Xcode 26+ / iOS 26+ SDK (in force since 28 Apr 2026) | Target API 36 (Android 16) for new apps and updates (in force since 31 Aug 2026) |
| This project already meets it? | ✅ builds with the iOS 27.1 SDK | ✅ targets API 36, minimum API 26 |

**Why an organization account?** The store shows the developer name under the app. With an organization account it says "Α.Σ. Απόλλων Βωλάδος" instead of a volunteer's personal name, the account survives board changes, and on Google Play it avoids the 14-day/12-tester closed test that new *personal* accounts must run (§3.1).

### Recommended order
1. Get the paperwork and the D-U-N-S number (§1). It's the slowest step, so start now.
2. Fix the pre-submission blockers in this repo (§5).
3. Enroll in both programs (§2.1, §3.1) in parallel.
4. Publish a privacy-policy page on apollonvolados.gr (§1.4).
5. Build, sign and upload (§2.4, §3.3), test with TestFlight / internal testing, then submit for review.

---

## 1. Before you start (once)

### 1.1 Legal paperwork the club needs to have ready
- **Court registration of the σωματείο** (decision of the Πρωτοδικείο and the registry entry) and the **current statute**.
- **ΑΦΜ** and the **registered address**, spelled exactly as they appear in official records.
- **Board decision** (απόφαση Δ.Σ.) naming the person who will be the *Account Holder* and authorising them to accept Apple's and Google's agreements for the club. Apple calls the person who enrolls and verifies that they "have legal authority to bind the organization".
- Proof of non-profit status (the statute plus registration) for Apple's fee waiver.

### 1.2 Club-owned contact details
Create these under the club's domain, not on someone's personal Gmail:
- `apps@apollonvolados.gr` (or similar) as the login for both stores. Turn on two-factor authentication, and keep the recovery codes with the club secretary.
- A phone number that stays with the club. Google verifies the contact phone by one-time code and requires it to stay working.
- `info@apollonvolados.gr` as the public support email. The app already uses it.

### 1.3 D-U-N-S number (free)
Both stores identify organizations by their D-U-N-S number from Dun & Bradstreet.

1. Use **Apple's D-U-N-S lookup tool** (developer.apple.com/support/D-U-N-S) to check whether the club already has a number.
2. If it doesn't, submit the club's details through the same tool. The number is **free**. Apple says to allow up to 5 business days; Google says up to 30 days.
3. In Greece, D&B is represented by **ICAP**. ⚠️ Users report that ICAP offers *paid* "fast" processing. You don't need it: the free route works, and Apple notes that paying does not shorten the wait.
4. Make sure the D-U-N-S record's **legal name and address match the court registration exactly**. Mismatches are the most common reason enrollment stalls.

### 1.4 Privacy policy page (required by both stores)
Publish a page such as `https://apollonvolados.gr/privacy` in Greek (English optional). For this app it can be short and true:
- The app **collects no personal data**, has no accounts, no analytics and no advertising.
- It downloads public news from apollonvolados.gr and keeps a temporary copy on the device so news opens offline. The copy expires after 6 hours.
- It saves one setting on the device: the appearance choice (Automatic/Light/Dark).
- Phone, email, website and map buttons open other apps; nothing is sent through the app.
- Contact: info@apollonvolados.gr.

If you later add analytics, push notifications, donations or sign-in, update the policy **and** both stores' privacy forms first.

### 1.5 Costs summary
| Item | Cost |
| --- | --- |
| D-U-N-S number | Free |
| Apple Developer Program | 99 USD/year, or 0 with an approved fee waiver |
| Google Play developer registration | 25 USD once |
| Mac for building iOS | Required (any Mac that runs Xcode 26+) |

---

## 2. Apple App Store

### 2.1 Enroll the club in the Apple Developer Program
1. The Account Holder signs in at **developer.apple.com/programs/enroll** with the club Apple Account (`apps@apollonvolados.gr`, two-factor on).
2. Choose **Organization**, enter the D-U-N-S number, the legal name (as on the D-U-N-S record) and the website `apollonvolados.gr`. The website must belong to the club.
3. Apple may phone the Account Holder to verify their authority. Have the board decision at hand.
4. Pay the 99 USD fee, **or** request a waiver first (§2.2).

### 2.2 Ask for the non-profit fee waiver
Apple waives the annual fee for organizations that:
- are a **legal entity with non-profit status** recognised by an authority in their country (for an EU club, Apple's example is "a local register of charities"),
- are **not** an individual, sole proprietor or single-person business,
- **do not sell anything** through their apps: no paid apps, no in-app purchases, and the Paid Applications Agreement not signed.

How: apply on Apple's **membership fee waiver request page** (linked from developer.apple.com/support/fee-waiver), attaching the statute and court registration. ⚠️ Apple's support page doesn't list eligible countries, and the last public list (2020) didn't include Greece. Apply anyway; if Greece isn't eligible, Apple will say so and you pay the normal fee.

Keep in mind: if the club later adds paid features or in-app donations, the waiver ends and the 99 USD fee applies. Donations through an external website link are a separate topic; read App Review Guideline 3.2.1(vi) on non-profit donations first.

### 2.3 Create the app record in App Store Connect
In **appstoreconnect.apple.com → Apps → +**:

| Field | Value |
| --- | --- |
| Platforms | iOS |
| Name | Apollon Volados (or Απόλλων Βωλάδος, max 30 characters) |
| Primary language | Greek |
| Bundle ID | `com.apollonvolados.mobile` (register it first under Certificates, IDs & Profiles → Identifiers). **It can never change.** |
| SKU | `apollonvolados-ios` (internal) |
| User access | Full access |

### 2.4 Signing and building the release
**One-time setup (on the Mac that builds):**
1. In Xcode → Settings → Accounts, add the club Apple Account and select the club team.
2. Create an **Apple Distribution** certificate (Manage Certificates → + → Apple Distribution).
3. In the developer portal, create an **App Store Connect** provisioning profile for `com.apollonvolados.mobile` and download it. Xcode can also manage it automatically.

**This repo's signing settings:** `ApollonVolados.Mobile.csproj` hard-codes `CodesignKey` to a certificate fingerprint from one developer's Mac. On any other Mac, or once the club's own certificate exists, override it on the command line instead of editing it each time:

```bash
dotnet publish ApollonVolados.Mobile/ApollonVolados.Mobile.csproj -f net10.0-ios -c Release -p:RuntimeIdentifier=ios-arm64 -p:ArchiveOnBuild=true -p:CodesignKey="Apple Distribution: Α.Σ. Απόλλων Βωλάδος (TEAMID)" -p:CodesignProvision="Apollon Volados App Store"
```

The `.ipa` lands in `ApollonVolados.Mobile/bin/Release/net10.0-ios/ios-arm64/publish/`.

**Version numbers:** in the `.csproj`, `ApplicationDisplayVersion` is what users see (`1.0`, `1.1`, …). `ApplicationVersion` is the build number and **must increase with every upload** (1, 2, 3, …), even for the same display version.

### 2.5 Upload
- **Transporter** (free from the Mac App Store): sign in with the club account, drag in the `.ipa`, and click Deliver.
- After 10–30 minutes the build appears in App Store Connect → TestFlight.

### 2.6 Test with TestFlight first
1. Add board members as **internal testers** (up to 100, no review needed).
2. For a wider group (players, parents), add **external testers**. The first build gets a short beta review.
3. Test on a real iPhone: all four tabs, news offline (airplane mode), the Club tab's contact buttons, light/dark switching, and the team pages (§5).

### 2.7 Fill in the store listing
| Item | What to enter |
| --- | --- |
| Subtitle (30 chars) | e.g. «Επίσημη εφαρμογή του συλλόγου» |
| Description | See the Greek draft in §4 |
| Keywords (100 chars) | `Απόλλων,Βωλάδος,Βωλάδα,Κάρπαθος,ποδόσφαιρο,σύλλογος,ομάδα,ακαδημία` |
| Support URL | `https://apollonvolados.gr` (a page with contact info) |
| Privacy Policy URL | the page from §1.4 |
| Category | Sports (primary); News (secondary, optional) |
| Copyright | `2026 Α.Σ. Απόλλων Βωλάδος` |
| App icon | Taken from the build automatically. A flattened 1024×1024 copy is in `docs/store/appstore-icon-1024.png`. |
| Screenshots | **iPhone 6.9"** (1320×2868 or 1290×2796), 3–10 images. Use the iPhone 17/18 Pro Max simulator: `xcrun simctl io booted screenshot shot.png`. |
| iPad screenshots | **Required**, because the app declares iPad support (`UIDeviceFamily` 1 and 2 in `Info.plist`): 13" iPad, 2064×2752. To skip iPad, remove `<integer>2</integer>` from `UIDeviceFamily` before the first submission. |

**App Privacy ("nutrition label"):** answer **"No, we do not collect data from this app"**. That's accurate for the current code (§1.4).

**Age rating:** complete the questionnaire. With no objectionable content and no user-generated content, the result is **4+**. Note that the WebView pages show content from the club website; if the questionnaire asks about unrestricted web access, answer truthfully. The app opens fixed club pages, not a general browser.

**Export compliance:** already answered in `Info.plist` (`ITSAppUsesNonExemptEncryption = false`). The app only uses standard HTTPS.

### 2.8 Submit for review
- In **App Review Information**, give a contact name, phone and email, and a note such as: *"Official app of the non-profit sports club Α.Σ. Απόλλων Βωλάδος (Volada, Karpathos, founded 1924). No login required. News comes from our WordPress site apollonvolados.gr."*
- Review usually takes 1–3 days. Choose **Manual release** so the board can pick the launch day.
- ⚠️ **Risk: Guideline 4.2 (minimum functionality).** Apple rejects apps that are "just a website in a wrapper". This app has native screens (news list, history, timeline, contact), but the article and team pages are WebViews. If Apple pushes back, reply in the Resolution Center explaining the native features. Longer term, render articles natively from the WordPress API instead of in a WebView.

---

## 3. Google Play

### 3.1 Create an organization developer account
1. Go to **play.google.com/console/signup** with the club Google account (`apps@apollonvolados.gr`; Google Workspace or a Google account created on that address).
2. Choose **"An organization or business"**.
3. Create or select a **Google payments profile of type "Organization"** with the **D-U-N-S number**. Google pulls the name and address from it, so they must match.
4. Fill in the developer name shown on Google Play (`Α.Σ. Απόλλων Βωλάδος`), website, contact name/email/phone (each verified by one-time code), and a public developer email (`info@apollonvolados.gr`).
5. Pay the **25 USD** one-time fee. Payment verification can take up to 5 days; Google may ask for a small deposit challenge or bank documents.
6. Google may ask for documents proving the organization exists. Use the court registration and statute.

Notes:
- Google won't create an organization account without a D-U-N-S number, and it grants no exemptions.
- ⚠️ The closed test of **12 testers for 14 days** before production is required for **personal** accounts created after 13 Nov 2023. Third-party guides say organization accounts are exempt, but I couldn't confirm that in Google's own text. If the Console still shows the requirement, recruit 12 board members, players or parents for a closed test.

### 3.2 Create the app
Play Console → **Create app**:

| Field | Value |
| --- | --- |
| App name | Apollon Volados / Απόλλων Βωλάδος (max 30 chars) |
| Default language | Greek – el-GR |
| App or game | App |
| Free or paid | **Free.** You can't switch a free app to paid later. |
| Package name | Comes from the first upload: `com.apollonvolados.mobile`. **It can never change.** |

### 3.3 Signing and building the release
Google Play requires an **Android App Bundle (`.aab`)**; the Release configuration already produces one. Google holds the final *app signing key* (Play App Signing), and you sign uploads with an *upload key* that you create once:

```bash
keytool -genkeypair -v -keystore apollonvolados-upload.keystore -alias apollonvolados -keyalg RSA -keysize 2048 -validity 10000
```

- **Never commit the keystore or its passwords.** `.gitignore` already ignores `*.keystore`/`*.jks`. Store the file and passwords in the club's password manager and keep an offline backup. If the upload key is lost, Google can reset it, but that takes days.

Build a signed bundle, passing the passwords from environment variables so they never appear in files:

```bash
export AV_KEY_PASS='…' AV_STORE_PASS='…'
```

```bash
dotnet publish ApollonVolados.Mobile/ApollonVolados.Mobile.csproj -f net10.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=$PWD/apollonvolados-upload.keystore -p:AndroidSigningKeyAlias=apollonvolados -p:AndroidSigningKeyPass=env:AV_KEY_PASS -p:AndroidSigningStorePass=env:AV_STORE_PASS
```

The signed `com.apollonvolados.mobile-Signed.aab` lands in `ApollonVolados.Mobile/bin/Release/net10.0-android/publish/`.

**Version numbers:** the same `.csproj` properties as iOS. `ApplicationVersion` becomes Android's `versionCode` and **must increase with every upload**.

### 3.4 Test tracks
1. **Internal testing:** upload the `.aab` and add up to 100 testers by email. Available within minutes, no review.
2. **Closed testing:** only if the Console requires it (§3.1), or for a wider beta.
3. **Production:** promote the tested release. The first review typically takes a few days, sometimes up to a week or more for new accounts.

### 3.5 Store listing and app content
**Main store listing:**

| Item | What to enter |
| --- | --- |
| Short description (80 chars) | «Νέα, ομάδες και ιστορία του Α.Σ. Απόλλων Βωλάδος από το 1924.» |
| Full description (4,000 chars) | See §4 |
| App icon | `docs/store/play-icon-512.png` (512×512, generated from the app icon) |
| Feature graphic | 1024×500 PNG/JPG, **required**. Use the crest on crest green `#007A3D`. |
| Phone screenshots | 2–8, 16:9 or 9:16, at least 1080 px on the short side. The Pixel 8 emulator works: `adb exec-out screencap -p > shot.png` |
| Category | Sports |
| Contact details | Email `info@apollonvolados.gr`, website, phone (optional) |

**App content (Policy → App content); complete every item:**
- **Privacy policy:** the URL from §1.4.
- **Ads:** "No, my app does not contain ads".
- **App access:** "All functionality is available without special access".
- **Content rating:** the IARC questionnaire. Expected result: PEGI 3 / Everyone.
- **Target audience:** choose **13+**, unless the board specifically wants to market the app to children. Choosing under-13 brings in the stricter **Families policy**.
- **Data safety:** "No data collected" and "No data shared". The cached news and appearance setting stay on the device, and Google doesn't count on-device-only data as "collected".
- **News apps declaration:** if Play asks whether this is a news app, answer **No**. It's a sports club app with club announcements; declaring it a news app adds publisher requirements.
- **Government app:** No. **Financial features:** None. **Health:** None.

---

## 4. Greek store description (draft)

> **Απόλλων Βωλάδος — η επίσημη εφαρμογή του συλλόγου**
>
> Ο Αθλητικός Σύλλογος Απόλλων Βωλάδος ιδρύθηκε το 1924 και κρατά ζωντανή την αθλητική παράδοση της Βωλάδας στην Κάρπαθο. Με την εφαρμογή μένετε κοντά στον σύλλογο, όπου κι αν βρίσκεστε.
>
> • **Νέα** – τα τελευταία νέα και ανακοινώσεις του συλλόγου, διαθέσιμα και χωρίς σύνδεση.
> • **Τμήματα** – ανδρική και γυναικεία ομάδα, ακαδημίες, μπάσκετ, Απόλλων Αττικής και Απόλλων USA.
> • **Σύλλογος** – η ιστορία μας από το 1924, οι σημαντικοί σταθμοί και τα στοιχεία επικοινωνίας.
> • **Φωτεινό ή σκοτεινό θέμα** – ή αυτόματα, ανάλογα με το τηλέφωνό σας.
>
> Χωρίς διαφημίσεις, χωρίς λογαριασμό, χωρίς συλλογή δεδομένων.

Have a board member proofread it before submitting.

---

## 5. Pre-submission checklist for this repo

Blockers, which reviewers will hit:
- [ ] **Fix the team links.** As of 1 Nov 2026, 5 of the 6 URLs in `ViewModels/TeamsViewModel.cs` return "page doesn't exist" on apollonvolados.gr; only `/antriki-omoada/` works. Broken screens are a common rejection reason (App Review Guideline 2.1, Google's broken-functionality policy). Either create the pages on the website or point the links at pages that exist.
- [ ] **Privacy policy page** is live (§1.4).
- [ ] **iPad:** provide 13" iPad screenshots, or drop iPad support (§2.7).
- [ ] **iOS signing:** build with the club's distribution certificate (§2.4), not the fingerprint hard-coded in the `.csproj`.

Should fix:
- [ ] Spelling: one team title says «Απόλλων **Βολάδος** Αττικής»; everywhere else it's «Βωλάδος».
- [ ] Check the launcher icon on a **real Android phone**. On the Pixel 8 emulator (Android 16), the launcher drew the icon background in a pale mint, although the generated layers are solid crest green `#007A3D`. Settings and iOS show it correctly.
- [ ] Decide on the contact phone number shown in the app (`ClubViewModel.PhoneNumber`). It's public once the app ships.

Before **every** release:
- [ ] Increase `ApplicationVersion` (and `ApplicationDisplayVersion` when user-visible things change).
- [ ] `dotnet test tests/ApollonVolados.Mobile.Tests` passes.
- [ ] Run on the iOS Simulator and an Android emulator in light and dark mode (README → *Build and run*).
- [ ] Test on at least one real iPhone (TestFlight) and one real Android phone (internal testing).
- [ ] Update the privacy policy and store privacy forms if data handling changed.

---

## 6. After launch

- **Who has access:** add the developer(s) as users in App Store Connect (role *App Manager* or *Developer*) and the Play Console (*Release manager*). Don't share the Account Holder login.
- **Renewal:** Apple's membership renews yearly, even if waived; the Account Holder gets an email beforehand. Each year Apple and Google also publish new SDK/target-API deadlines that require rebuilding with the current .NET MAUI and Xcode.
- **Agreements:** both stores periodically require the Account Holder to accept updated agreements; uploads are blocked until they do.
- **Reviews:** reply to store reviews from the club account.
- **Automation (optional):** the GitHub Actions workflow (`.github/workflows/build-ios.yml`) builds an unsigned `.ipa` and runs the tests. To publish from CI, add the distribution certificate, provisioning profile, upload keystore and passwords as **GitHub encrypted secrets**, never in the repo.

---

## Sources (checked 1 Nov 2026)

- Apple: [Developer Program fee waiver](https://developer.apple.com/support/fee-waiver/) · [D-U-N-S Number](https://developer.apple.com/support/D-U-N-S/) · [Upcoming SDK minimum requirements](https://developer.apple.com/news/upcoming-requirements/)
- Google: [Requirements for creating an organization developer account (D-U-N-S)](https://support.google.com/googleplay/android-developer/answer/13628312?hl=en) · [Target API level requirements](https://developer.android.com/google/play/requirements/target-sdk) · [App testing requirements for new personal accounts](https://support.google.com/googleplay/android-developer/answer/14151465?hl=en-GB)
- Fee-waiver country list (2020): [AppleInsider](https://appleinsider.com/articles/20/02/03/apple-extends-developer-program-fee-waivers-to-8-countries)
- D-U-N-S in Greece via ICAP (user reports): [B4X forum](https://b4x.com/android/forum/threads/google-play-developer-verification-deadline-email-d-u-n-s.157324/)
- Organization accounts and the 12-tester rule (third-party): [Choicely](https://www.choicely.com/blog/google-play-12-tester-rule) · [ExtendsClass](https://extendsclass.com/blog/google-plays-closed-testing-requirement-what-developers-need-to-know-in-2026)
