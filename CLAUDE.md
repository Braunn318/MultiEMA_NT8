# MultiEMA Indicator NT8 — Working Notes

- Master copy of the indicator is `MultiEMA.cs` in this folder (git repo, pushed to `github.com/Braunn318/MultiEMA_NT8`, public).
- Install: copy `MultiEMA.cs` to `Documents\NinjaTrader 8\bin\Custom\Indicators\Braunn\`, then NinjaScript Editor → F5. Keep the installed copy identical to the one here.
- Only the indicator lives here – never add the M2 Trend AOS strategy or M2Zones to this repo.
- Branches: `main` = stable, `test` = in-progress work, merged into `main` once the user verifies it in NT8.
- One changelog only: `CHANGELOG.txt`, newest on top, `## <version> - <date>`. Also bump the version note in the `MultiEMA.cs` header and the README "Historie".
- Releases like M2Zones (`F:\Trading\DEV\M2 Indicator\.claude\skills\release_*`): compile check against the NT8 DLLs (generated-code region cut off), tags `vX.Y.Z-test` (pre-release on `test`) and `vX.Y.Z` (Latest on `main`), asset `MultiEMA.cs`.
- Repo is public: no account data, API keys or real trade numbers in tracked files.
