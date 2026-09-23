# 3. Build the Test Planner Agent

Give your coding agent this instruction:

```
Using the UiPath agent skill, generate a low-code agent named
"test-planner-agent".

Inputs:
- in_ChangeRequestPDF — the change request document (PDF)
- in_RiskAssessmentJSON — structured risk assessment (JSON), which may
  contain reviewer overrides: risk_level_final, override_applied,
  reviewer_notes

Design a test plan for the BenefitConnect enrollment wizard, scoped ONLY to
these wizard steps: login, plan_selection, dependents, beneficiaries,
life_events, confirm. Do not propose tests outside the enrollment wizard.

For each scenario:
- Base priority on the risk factors in in_RiskAssessmentJSON. A "high" or
  "critical" factor tied to validation logic (e.g. beneficiary percentages,
  life-event type rules) must produce at least one "high" priority scenario
  covering that validation.
- Use risk_level_final if present, otherwise the original risk level.
- Do not invent business rules not stated in the inputs — flag assumptions
  in coverage_notes instead.

Return output matching this structure:

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
        "action": "string — what should be done, step by step",
        "expected_result": "string — what to verify"
      }
    ],
    "coverage_notes": "string"
  }
}
```

Publish and run this agent the same way as in [Module 1](../module-1-change-risk-assessor.md): upload it with
`uip solution upload`, then run it in Studio Web.
