# 4. ⚠️ Register and run the plan in Test Manager

!!! warning "Preview capability — not yet verified end to end"
    This step uses UiPath Test Manager (`uip tm`), which is in Preview. The commands below come from the
    `uipath-test` skill's documented commands and haven't been run end to end. Confirm each step works in your
    tenant before delivering this live. Also check that `link-automation` supports per-scenario runtime inputs
    the way this page assumes, and adjust if it doesn't.

Ask your coding agent to take `out_TestPlanJSON` and do the following:

1. **Create or reuse a Test Manager project.** Run `uip tm project list --filter <name>`, then
   `uip tm project create` if you need a new one.
2. **Create a test case and its steps for each scenario:**
   ```
   uip tm testcases create --project-key <KEY> --name <scenario title>
   uip tm testcases steps add --project-key <KEY> --test-case-id <id> --description <action>
   ```
3. **Group the test cases into a test set:**
   ```
   uip tm testsets create --project-key <KEY> --name "Enrollment Wizard — <change_id>"
   uip tm testcases add --test-set-key <KEY> --test-case-keys <...>
   ```
4. **Link each test case to the automation:**
   ```
   uip tm testcases link-automation --project-key <KEY> --test-case-key <KEY> --folder-key <FOLDER> --package-name Run-EnrollmentWizard-Test --test-name <name>
   ```
5. **Run the test set.** This returns an execution ID:
   ```
   uip tm testsets run --test-set-key <KEY>
   ```
6. **Get the report:**
   ```
   uip tm report get --execution-id <ID> --project-key <KEY>
   ```

!!! tip "Open Test Manager in the browser"
    Don't do this whole step from the CLI. Check the Test Manager UI at three points:

    - **After steps 2–3:** see the test cases and the test set with linked automation
    - **After step 5:** watch each case pass or fail live
    - **After step 6:** read the report, which is tailored to each persona
