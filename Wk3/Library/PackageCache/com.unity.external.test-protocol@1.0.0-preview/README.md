# Unity Test Protocol

Automated Testing at Unity includes many components. We want all the components to exchange test execution data
using a simple, well defined language - Unity Test Protocol or just UTP.

Having UTP would help us to:

- detect errors based on well-defined rules;
- build better test reporting;
- make more analysis tools for text execution data;
- integrate with existing analysis tools (e.g., with chrome://tracing).


## Basic principles

Each UTP message:

- is represented in JSON format;
- has a unique type. E.g. `ArtifactPublish`, `TestStatus`, `TestSession`, etc;
- has a timestamp in millisecond since epoch in UTC;
- has a phase (see Message types below);
- has an array of errors (empty by default);
- and if written to log with non-UTP messages, should be prefixed by `##utp:`.

## Message types

There are two fundamental types of UTP messages - Instant and Begin-End messages.

### Instant messages

Instant messages are used to represent 'instant' events, which do not have any meaningful duration.
For example, `ArtifactPublish` message:

```
{
  "type": "ArtifactPublish",
  "phase": "Immediate",
  ...
  "destination": "C:\\buildslave\\unity\\build\\build\\ReportedArtifacts\\TestRunnerLog.txt"
}
```

The syntax `phase=Immediate` is used to mark instant messages.

### Begin-end messages

Begin-End UTP messages is used to represent synchronous actions, defined by a `phase=Begin, phase=End` pair.
The Begin Message must come before the corresponding End Message.
You can nest Begin-End messages. Instant messages can also be nested
into Begin-End messages.

For example:

```
{
  "type": "TestSession",
  "time": 1519344091036,
  "phase": "Begin",
  "version": 2,
  "processId": 75305,
  "errors": [],
  "minimalCommandLine": [
    "--suite=graphics",
    "--platform=StandaloneOSX",
    "--configuration=glcore",
    "-owner=yan yan@unity3d.com"
  ]
}

{
  "type": "ArtifactPublish",
  "time": 1519344091044,
  "phase": "Immediate",
  "version": 2,
  "processId": 75305,
  "errors": [],
  "destination": "/Users/builduser/buildslave/unity/build/build/ReportedArtifacts/TestRunnerLog.txt"
}

{
  "type": "Action",
  "time": 1519344092749,
  "phase": "Begin",
  "version": 2,
  "processId": 75305,
  "errors": [],
  "name": "projectBuild",
  "description": "Building project for StandaloneOSX_glcore_Gamma"
}

{
  "type": "Action",
  "time": 1519349381543,
  "phase": "End",
  "version": 2,
  "processId": 75305,
  "errors": [],
  "name": "projectBuild",
  "description": "Building project for StandaloneOSX_glcore_Gamma",
  "duration": 5288792
}
...

{
  "type": "TestSession",
  "time": 1519350193061,
  "phase": "End",
  "version": 2,
  "processId": 75305,
  "errors": []
}
```

In this example, `TestSession` is a top level Begin-End message.
It has one nested instant message - `ArtifactPublish` - and one nested Begin-End message  - of type `Action`.
Using this structure, we can build a test report like this:

```
TestSession
    ArtifactPublish: TestRunnerLog.txt
    Building project for StandaloneOSX_glcore_Gamma 1:46
```

In practice, test execution structure is complex and includes more message types.
But still, nested messages define a tree structure, which represents a hierarchy of actions
in a test execution.

One way we can use it is to build a hierarchical test report:

![Test Report](https://i.imgur.com/l4g5iEA.png])

Or we can generate data compatible with other tools. E.g. with Google trace tool:

![chrome://tracing](https://i.imgur.com/p0pWo0y.png])


## Merging begin-end messages

`Begin` and `End` messages can be merged. Once merged, the result message has `phase=Complete`.
Merge rules are:
  - if the begin or end message has a `name` field it must be the same in both messages; The only exception is a `TerminateCurrent` message (see below).
  - errors of End Message are appended to errors of Begin message;
  - if duration is not specified, it will be calculated as: `endMessage.time - beginMessage.time`

Sometimes, it is impossible to receive a proper End message. For example, if we ran editor tests, but the editor hangs and the latest message we get back is `{"type":"TestStatus", "Phase":"Begin"...}` we should be able to terminate the current action.
For such cases, there is a special message  `TerminateCurrent`. It always has `phase=End`.
Any Begin message can be merged with `TerminateCurrent`. A `TerminateCurrent` message must come
with an error explaining the reason for termination.

Learn more about message merge rules in
`Tests/Unity.TestProtocol.Tests/MergeMessageTests.cs`

#Source code reference

Source code for Unity Test Protocol is located in `Tests/Unity.TestProtocol`
Nice way to learn how it works is by looking into tests: `Tests/Unity.TestProtocol.Tests`

#Feedback

Please give any feedback in our Slack channel, `#devs-test-platform`.
