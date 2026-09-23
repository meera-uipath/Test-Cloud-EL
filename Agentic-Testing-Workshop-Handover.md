# Agentic Testing Workshop — Handover

Live planning doc (source of truth, keep updating there): https://claude.ai/artifact/ELSjMVPaKWzFiT5SJGYeWc

As of: 2026-09-23

## What this is

A hands-on workshop teaching participants to build a chain of coding-agent-built agents — risk assessment, test
planning, and test execution/reporting — orchestrated in Maestro against a live demo app (BenefitConnect /
Accrual Bank), inspired by UiPath's "dark testing factory" concept video.

Reference workshop this is modeled on (structure + mechanics reused): https://uipath-practice.github.io/AgenticCodingAgents/
— specifically its "Getting Started with Coding Agents" lessons (CLI practice, build-your-first-agent loop) and
the Benefit Claims Processing workshop's BPMN/Maestro/Action App pattern.

Source material: UiPath internal "Dark Testing Factory" concept video (Slack, transcript captured in this
session) — describes an autonomy ladder (manual → rule-based automation → conversational agents → connected
agentic workflows → fully autonomous "dark factory"), a risk-assessor sub-agent, human-in-the-loop review, and
an AI Trust Layer for governance.

## Test target application

**BenefitConnect Portal** — `https://accrual-bp-hri.azurewebsites.net/login`
Source repo: `DemoApplications-main/BenefitsPortal` (in `OneDrive - UiPath/Training Docs/Coding Agents/DemoApplications-main/DemoApplications-main`)

- Full HR benefits-enrollment demo app for a fictional "Accrual Bank" tenant
- Rich UI: login → dashboard → 6-step enrollment wizard (plan selection, dependents, beneficiaries, life
  events, documents) → confirm
- Documented REST API (`API.md`) with a "killswitch" feature gating third-party API access (not used in this
  workshop's current scope, but available if extended)
- Real testable business logic: beneficiary allocation must sum to 100%, life-event submissions must use a
  valid QLE type, role-based visibility (HR Admin vs. Global Admin)
- Demo accounts (all password `Accrual!2026` unless noted): `jordan`, `leilani` (new hire, no existing
  enrollment — good for onboarding-style scenarios), `priya`, `marcus`; admin is `admin`/`admin`
- Was originally built for a different RPA demo narrative ("Alex" the onboarding bot pivoting from API to UI
  automation when the killswitch flips) — we are reusing the app, not that narrative
- Other folders in the same repo (`Bank_system`, `ERP_System`, `HR_System`) are unrelated demo apps — not used here

## Sample scenarios (used consistently across modules)

- **Low-risk:** "Update the button label on enrollment wizard step 3 from 'Continue' to 'Next'" — expected: low
  risk, no escalation, straight through to test planning + execution
- **High-risk:** "Add a new Life Event type ('Adoption') to the enrollment wizard's Life Events step" —
  expected: high/critical risk, escalates to human review before test planning

## Module 1 — Change Risk Assessor Agent

Goal: teach the describe → generate → validate → publish → run loop with one simple, single-purpose agent that
scores the business risk of an incoming change to BenefitConnect.

**Inputs**
- `in_ChangeRequestPDF` — the change request / user story
- `in_SystemContextPDF` — short doc on the affected BenefitConnect area's business criticality / compliance sensitivity

**Risk checks (5 deterministic dimensions)**
1. Business criticality
2. Change scope
3. Regulatory/compliance sensitivity
4. Test coverage gap
5. Change type risk (new feature vs. modifying existing critical logic vs. bug fix)

**Output schema**
```json
{
  "out_riskLevel": "low | medium | high | critical",
  "out_riskSummary": "string",
  "out_riskAssessmentJSON": {
    "change_id": "string",
    "escalate": "boolean",
    "risk_factors": {
      "business_criticality": { "result": "low|medium|high", "detail": "string" },
      "change_scope": { "result": "low|medium|high", "detail": "string" },
      "compliance_sensitivity": { "result": "low|medium|high", "detail": "string" },
      "coverage_gap": { "result": "low|medium|high", "detail": "string" },
      "change_type_risk": { "result": "low|medium|high", "detail": "string" }
    },
    "recommended_test_priority": "string"
  }
}
```

Escalation rule: any `critical` factor → `escalate = true`.

**Fallback for participants:** a known-good pre-built version of this agent's output (and optionally the agent
itself) should be provided so a broken Module 1 build doesn't block Module 2.

## Module 2 — Orchestrating Risk → Test Plan → Test Manager Execution

Goal: chain the risk assessor into a Maestro-orchestrated flow with a human checkpoint, then a test-planner
agent, then execute/report through **real UiPath Test Manager (`uip tm`)** — not a custom hand-rolled reporting
agent (this was a deliberate late change — see "Design decisions" below).

**Provided (instructor) vs. built (participants)**

| Provided | Built by participants |
|---|---|
| Maestro/BPMN process shape | `test-planner-agent` |
| Action App ("Risk Review") | Wiring agents + Test Manager into the process |
| `Run-EnrollmentWizard-Test` RPA workflow | (Optional Module) their own version of it |
| Known-good risk-assessor output (fallback) | |

**BPMN shape**
```
Start (change request received)
  → Agent Task: change-risk-assessor-agent
  → Gateway: escalate == true?
      yes → Human Task (Action App: Risk Review)
      no  → skip
  → Agent Task: test-planner-agent
  → uip tm: register test cases + test set
  → uip tm: link-automation to Run-EnrollmentWizard-Test
  → uip tm: testsets run
  → uip tm: report get
  → End (shareable test report)
```

**Action App ("Risk Review") — human-readable form, never raw JSON**
- Read-only: change title, risk level badge, plain-language summary, bulleted risk factors + detail,
  recommended test priority
- Reviewer input: Decision (`Approve as assessed` / `Override risk level`); if overriding, a Risk Level dropdown
  + required Notes field
- On submit, writes back `{ risk_level_final, override_applied, reviewer_notes, reviewer, timestamp }` into
  process data — this is the human-form → structured-data mapping; testers never touch JSON directly

**`test-planner-agent` (participants build this)**
- Inputs: `in_ChangeRequestPDF`, `in_RiskAssessmentJSON` (may carry reviewer overrides)
- Scoped ONLY to enrollment-wizard steps: `login | plan_selection | dependents | beneficiaries | life_events | confirm`
- Priority driven by risk factors (a high/critical validation-related factor forces at least one high-priority
  scenario covering that validation)
- Output:
```json
{
  "out_TestPlanSummary": "string",
  "out_TestPlanJSON": {
    "change_id": "string",
    "test_scenarios": [
      {
        "id": "TC-01",
        "title": "string",
        "priority": "high | medium | low",
        "wizard_step": "login | plan_selection | dependents | beneficiaries | life_events | confirm",
        "persona": "jordan | leilani | priya | marcus",
        "action": "string",
        "expected_result": "string"
      }
    ],
    "coverage_notes": "string"
  }
}
```

**Execution + reporting via real Test Manager (`uip tm`)**
1. Create/reuse a Test Manager project; turn each scenario into `uip tm testcases create` + `steps add`
2. Group into a test set (`uip tm testsets create`, `testcases add`)
3. `uip tm testcases link-automation` — links the case to the published `Run-EnrollmentWizard-Test` process,
   passing `wizard_step`/`persona`/`action`/`expected_result` as inputs
4. `uip tm testsets run` — executes, returns an execution ID
5. `uip tm report get` — persona-tailored (QA engineer / developer / release-manager) shareable report

⚠️ `uipath-test` / Test Manager CLI is **Preview** status — dry-run this sequence before finalizing.

**Explicit "go look at the product" checkpoints (don't let this stay CLI-only)**
- After steps 2–3: open Test Manager UI, see the test cases + test set with linked automation
- After step 4: open the execution in the UI, watch live pass/fail per case
- After step 5: open the persona-tailored report in the UI and read it

**Run both sample scenarios end-to-end** and confirm the low-risk one skips the Action App while the high-risk
one pauses for review before resuming into real RPA-driven Test Manager execution.

## Optional Module — Build the Enrollment Wizard Test Robot with a Coding Agent

Goal: instead of using the instructor-provided `Run-EnrollmentWizard-Test`, use Claude Code with the
`uipath-rpa` skill inside UiPath **Studio Desktop** to build it. Optional; Windows-only.

**Extra prerequisites:** Studio Desktop installed + signed in; `uipath-rpa` confirmed present after
`uip skills install`; authenticated to the workshop tenant.

**Key constraint:** a coding agent can reason/write XAML but can't visually browse a live page the way it reads
a PDF. The skill is "discovery-first" — selectors/UI targets must be captured before generation.

**Steps**
1. `mkdir enrollment-wizard-robot` — open in both Studio Desktop and the coding agent's terminal
2. Capture target screens first: manually walk login → plan selection → dependents → beneficiaries → life
   events → confirm using Studio's UI Explorer/Recorder against the live site (sign in as `jordan`), saving
   elements to the Object Repository
3. Prompt the coding agent (via `uipath-rpa`) to generate a dispatcher workflow that switches on `in_WizardStep`,
   routes to one child workflow per step, uses the captured selectors, wraps each step in Try/Catch (caught
   exception → `status = "blocked"`, never a false pass), validates the known business rules (beneficiary
   sum-to-100, valid QLE type), and returns `{ id, status: pass|fail|blocked, actual_result, evidence }`
4. Watch it scaffold — approve each step
5. Debug locally in Studio Desktop — confirm a real pass, a real fail, and a real blocked at least once each
6. Publish to Orchestrator as `Run-EnrollmentWizard-Test`
7. Re-wire Module 2 via `uip tm testcases link-automation`, re-run both sample scenarios

⚠️ Exact CLI/publish subcommands are inferred from the `uipath-rpa` skill's catalog description, not verified
end-to-end — confirm against live `uip --help` / skill guidance during a dry run.

## Components and Products Used

| Component | Used in |
|---|---|
| Coding agent (Claude Code) | All modules |
| `uip` CLI | All modules |
| UiPath skills: `uipath-agents`, `uipath-rpa`, `uipath-maestro-bpmn`, `uipath-human-in-the-loop`, `uipath-test`, `uipath-solution` | Modules 1–3 respectively |
| Low-code Agent Builder | Module 1, Module 2 (test-planner-agent) |
| Studio Web | Module 1 (publish/run/debug) |
| Maestro (BPMN) | Module 2 |
| Action Apps (human-in-the-loop) | Module 2 |
| UiPath Test Manager (`uip tm`) — **Preview** | Module 2 (execution + reporting) |
| Studio Desktop | Optional Module (Windows only) |
| Orchestrator | Publishing/running the RPA process |
| BenefitConnect demo app | Test target, Modules 1–3 |

**Not in scope:** Integration Service (real triggers), IXP (not needed — agents read PDFs directly), AI Trust
Layer / governance controls, Test Cloud / Test Scout (the video's own product surface for designing the
"factory" — distinct from this coding-agent build exercise).

## Design decisions worth knowing (why things are shaped this way)

- **One consistent app across all modules** (BenefitConnect) instead of a generic/synthetic domain, so the
  workshop tells one coherent story end-to-end, mirroring how the reference workshop uses one Benefit Claims
  domain throughout.
- **Pre-built Maestro process + Action App**, provided by instructor to save build time without cutting scope —
  participants still walk the canvas and inspect the Action App to learn the mechanics, they just don't build
  them from scratch.
- **Action App is a human-readable form, not a JSON editor** — testers approve/override via dropdowns and a
  notes field; the JSON mapping happens behind the scenes. This was an explicit correction from an earlier draft
  that assumed testers could work with raw JSON.
- **Execution/reporting moved to real UiPath Test Manager (`uip tm`)** instead of a custom hand-rolled
  test-execution-agent with a bespoke JSON report contract — discovered mid-planning that `uipath-test` already
  provides test case/set/execution management and persona-tailored reporting natively. This is both more
  realistic and less custom code for participants to trust. Tradeoff: it's a Preview-status capability.
- **RPA-building-via-coding-agent is optional and separate** from the main flow — it requires Windows + Studio
  Desktop (unlike the rest of the workshop, which can run from a coding agent terminal + Studio Web), and it
  introduces the one place a coding agent needs a manual "discovery" step (capturing selectors) before it can
  generate anything.
- **Fallback assets** (known-good agent output, pre-built process/app) exist specifically so one participant's
  broken build in an earlier module doesn't block them from completing later modules.

## Open items / not yet decided

- [ ] Total workshop time budget — determines whether the Optional Module fits in the same session or needs its own
- [ ] Dry-run the `uip tm` sequence end-to-end before finalizing Module 2 content (Preview status, may shift)
- [ ] Verify exact `uipath-rpa` scaffold/publish CLI syntax against live `uip --help` before finalizing the Optional Module
- [ ] Build/confirm the pre-made Maestro process and pre-built Action App instructor assets
- [ ] Decide whether to explore the `uipath-test` skill's Playwright packaging path as a lighter-weight
      alternative/complement to RPA-based execution
