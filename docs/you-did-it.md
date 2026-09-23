# You did it!

You've built a chain of coding-agent-built agents that handles risk assessment, test planning, and (through
UiPath Test Manager) test execution and reporting, all orchestrated end to end against a live application.
You'll use the same describe → generate → validate → publish → run loop for every agent you build from here. In
this workshop you applied it across a full testing workflow instead of a single agent.

## Next steps

- Run the flow on a change request you write yourself, not just the two samples
- Try the `uipath-test` skill's Playwright packaging path as a lighter alternative to RPA for web-only scenarios
- Start the flow from a real trigger (an Integration Service webhook, or a Jira story closing) instead of
  supplying a change request by hand
- Send feedback on anything that felt rough. Test Manager (`uip tm`) and `uipath-rpa` are both in Preview and
  still changing.
