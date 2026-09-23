# Module 2 — Orchestrate Risk → Test Plan → Test Manager Execution

**Goal:** chain Module 1's agent into a Maestro-orchestrated flow. Escalated changes get a human risk review.
Then a new test-planner agent designs test scenarios, and UiPath Test Manager runs them against the live
BenefitConnect enrollment wizard and reports the results.

## What's provided and what you build

| Provided by your instructor | Built by you |
| --- | --- |
| Maestro/BPMN process shape | `test-planner-agent` |
| "Risk Review" Action App | Wiring the agents and Test Manager into the process |
| `Run-EnrollmentWizard-Test` RPA workflow | (Optional Module) your own version of it |
| Known-good risk-assessor output (fallback) | |

## Steps

1. [Walk the pre-built Maestro process](1-walk-the-maestro-process.md)
2. [Inspect the "Risk Review" Action App](2-risk-review-action-app.md)
3. [Build the Test Planner Agent](3-test-planner-agent.md)
4. [Register and run the plan in Test Manager](4-test-manager.md) ⚠️
5. [Run both sample scenarios end-to-end](5-run-end-to-end.md)
