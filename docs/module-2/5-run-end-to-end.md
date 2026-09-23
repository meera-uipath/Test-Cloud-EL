# 5. Run both sample scenarios end-to-end

## Low risk: `CR-1001-button-label.pdf`

Check that it skips the Action App completely and goes straight to test planning and Test Manager execution.

## High risk: `CR-2004-adoption-life-event.pdf`

Check that it:

1. Pauses at the Action App
2. Resumes after you submit a review decision, and moves into test planning with your risk level (overridden, if you chose to)
3. Runs real RPA-driven checks against the live enrollment wizard
4. Produces a pass/fail report in Test Manager

!!! tip "Stuck on Module 1's output?"
    Swap in the known-good version from your instructor so you can still finish Module 2.
