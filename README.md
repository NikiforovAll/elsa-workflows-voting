# Elsa Worflow. Parallel Voting Workflow

The goal of this repository is to demonstrate the issue with the execution context and how it is restored from the database.

Basically, all kinds of implementations:

* `QuorumVotingWorkflow.cs`
* `QuorumVotingWorkflowComposite.cs`
* `QuorumVotingWorkflowCompositeActivity.cs`

Fail because of the same issue - the variables are not restored correctly as dynamic variables, although we can see them as part of `[Workflows].[Elsa].[WorkflowInstances]` instance.

For example, let's take a `QuorumVotingWorkflow.cs`:

Here is workflow definition:

```json
{
    "activities": [
        {
            "path": {
                "typeName": "String",
                "expression": {
                    "type": "Literal",
                    "value": "/parallel-quorum-vote"
                }
            },
            "supportedMethods": {
                "typeName": "String[]",
                "expression": {
                    "type": "Literal",
                    "value": [
                        "POST"
                    ]
                }
            },
            "authorize": {
                "typeName": "Boolean",
                "expression": {
                    "type": "Literal",
                    "value": false
                }
            },
            "policy": {
                "typeName": "String",
                "expression": {
                    "type": "Literal"
                }
            },
            "requestTimeout": null,
            "requestSizeLimit": null,
            "fileSizeLimit": null,
            "allowedFileExtensions": null,
            "blockedFileExtensions": null,
            "allowedMimeTypes": null,
            "exposeRequestTooLargeOutcome": false,
            "exposeFileTooLargeOutcome": false,
            "exposeInvalidFileExtensionOutcome": false,
            "exposeInvalidFileMimeTypeOutcome": false,
            "parsedContent": {
                "typeName": "Object",
                "memoryReference": {
                    "id": "Workflow1:variable-1"
                }
            },
            "files": null,
            "routeData": null,
            "queryStringData": null,
            "headers": null,
            "result": null,
            "id": "HttpEndpoint1",
            "nodeId": "Workflow1:Sequence1:HttpEndpoint1",
            "name": null,
            "type": "Elsa.HttpEndpoint",
            "version": 1,
            "customProperties": {
                "source": "QuorumVotingWorkflow.cs:30",
                "canStartWorkflow": true
            },
            "metadata": {}
        },
        {
            "statusCode": {
                "typeName": "System.Net.HttpStatusCode, System.Net.Primitives",
                "expression": {
                    "type": "Literal",
                    "value": "OK"
                }
            },
            "content": {
                "typeName": "Object"
            },
            "contentType": {
                "typeName": "String",
                "expression": {
                    "type": "Literal",
                    "value": "application/json"
                }
            },
            "responseHeaders": {
                "typeName": "Elsa.Http.Models.HttpHeaders, Elsa.Http",
                "expression": {
                    "type": "Literal",
                    "value": {}
                }
            },
            "id": "WriteHttpResponse1",
            "nodeId": "Workflow1:Sequence1:WriteHttpResponse1",
            "name": null,
            "type": "Elsa.WriteHttpResponse",
            "version": 1,
            "customProperties": {
                "source": "QuorumVotingWorkflow.cs:37"
            },
            "metadata": {}
        },
        {
            "items": {
                "typeName": "Object"
            },
            "body": {
                "activities": [
                    {
                        "taskName": {
                            "typeName": "String",
                            "expression": {
                                "type": "Literal",
                                "value": "PromptVote"
                            }
                        },
                        "payload": {
                            "typeName": "ObjectDictionary"
                        },
                        "result": {
                            "typeName": "Object",
                            "memoryReference": {
                                "id": "Workflow1:variable-2"
                            }
                        },
                        "id": "RunTask1",
                        "nodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                        "name": null,
                        "type": "Elsa.RunTask",
                        "version": 1,
                        "customProperties": {
                            "source": "QuorumVotingWorkflow.cs:50"
                        },
                        "metadata": {}
                    },
                    {
                        "variable": {
                            "id": "Workflow1:variable-3",
                            "name": "Variable_2",
                            "typeName": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Boolean, System.Private.CoreLib]], System.Private.CoreLib",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        },
                        "value": {
                            "typeName": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Boolean, System.Private.CoreLib]], System.Private.CoreLib"
                        },
                        "id": "SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E1",
                        "nodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E1",
                        "name": null,
                        "type": "Elsa.SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E",
                        "version": 1,
                        "customProperties": {
                            "source": "QuorumVotingWorkflow.cs:58"
                        },
                        "metadata": {}
                    }
                ],
                "variables": [],
                "id": "Sequence2",
                "nodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "name": null,
                "type": "Elsa.Sequence",
                "version": 1,
                "customProperties": {
                    "source": "QuorumVotingWorkflow.cs:46"
                },
                "metadata": {}
            },
            "id": "ParallelForEach\u003CUser\u003E1",
            "nodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
            "name": null,
            "type": "Elsa.ParallelForEach\u003CUser\u003E",
            "version": 1,
            "customProperties": {
                "source": "QuorumVotingWorkflow.cs:42"
            },
            "metadata": {}
        },
        {
            "votes": {
                "typeName": "Boolean[]"
            },
            "result": {
                "typeName": "Boolean",
                "memoryReference": {
                    "id": "Workflow1:variable-4"
                }
            },
            "id": "QuorumVote1",
            "nodeId": "Workflow1:Sequence1:QuorumVote1",
            "name": null,
            "type": "Elsa.Demo.API.Workflows.QuorumVote",
            "version": 1,
            "customProperties": {},
            "metadata": {}
        },
        {
            "condition": {
                "typeName": "Boolean"
            },
            "then": {
                "text": {
                    "typeName": "String"
                },
                "id": "WriteLine1",
                "nodeId": "Workflow1:Sequence1:If1:WriteLine1",
                "name": null,
                "type": "Elsa.WriteLine",
                "version": 1,
                "customProperties": {
                    "source": "QuorumVotingWorkflow.cs:72"
                },
                "metadata": {}
            },
            "else": {
                "text": {
                    "typeName": "String"
                },
                "id": "WriteLine2",
                "nodeId": "Workflow1:Sequence1:If1:WriteLine2",
                "name": null,
                "type": "Elsa.WriteLine",
                "version": 1,
                "customProperties": {
                    "source": "QuorumVotingWorkflow.cs:73"
                },
                "metadata": {}
            },
            "result": null,
            "id": "If1",
            "nodeId": "Workflow1:Sequence1:If1",
            "name": null,
            "type": "Elsa.If",
            "version": 1,
            "customProperties": {
                "source": "QuorumVotingWorkflow.cs:70"
            },
            "metadata": {}
        }
    ],
    "variables": [],
    "id": "Sequence1",
    "nodeId": "Workflow1:Sequence1",
    "name": null,
    "type": "Elsa.Sequence",
    "version": 1,
    "customProperties": {
        "source": "QuorumVotingWorkflow.cs:26"
    },
    "metadata": {}
}
```

Let's try to run it, see `workflow.http`:


Here is the result before we try to submit any kind of result of `RunTask` activity:

```json
{
    "$id": "1",
    "id": "7d347caaac3cb6a8",
    "definitionId": "QuorumVotingWorkflow",
    "definitionVersionId": "cd7291eace9fe745",
    "definitionVersion": 1,
    "status": "Running",
    "subStatus": "Suspended",
    "bookmarks": {
        "$id": "2",
        "$values": [
            {
                "$id": "3",
                "id": "aa9d9ff9925851c7",
                "name": "Elsa.RunTask",
                "hash": "45CE21E25A865687B8AC60072C0ACF221BEC9E171B40938E72CFBB2705AD8314",
                "payload": {
                    "$id": "4",
                    "taskId": "23a4c44e2833e724",
                    "taskName": "PromptVote",
                    "_type": "Elsa.Workflows.Runtime.Bookmarks.RunTaskBookmarkPayload, Elsa.Workflows.Runtime"
                },
                "activityId": "RunTask1",
                "activityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "activityInstanceId": "c2547d312d23f76",
                "createdAt": "2024-04-18T12:27:32.2106771+00:00",
                "autoBurn": true,
                "callbackMethodName": "ResumeAsync",
                "autoComplete": true
            },
            {
                "$id": "5",
                "id": "f18cc5c0ab31ee7",
                "name": "Elsa.RunTask",
                "hash": "42BA89CC4020B206D2A788EF011E1A551DA69A4013234C2AAF824F8747E3046D",
                "payload": {
                    "$id": "6",
                    "taskId": "2b0778610e37f35a",
                    "taskName": "PromptVote",
                    "_type": "Elsa.Workflows.Runtime.Bookmarks.RunTaskBookmarkPayload, Elsa.Workflows.Runtime"
                },
                "activityId": "RunTask1",
                "activityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "activityInstanceId": "f3ed169633664dbb",
                "createdAt": "2024-04-18T12:27:32.2130232+00:00",
                "autoBurn": true,
                "callbackMethodName": "ResumeAsync",
                "autoComplete": true
            }
        ]
    },
    "incidents": {
        "$id": "7",
        "$values": []
    },
    "isSystem": false,
    "completionCallbacks": {
        "$id": "8",
        "$values": [
            {
                "$id": "9",
                "ownerInstanceId": "184693755396caa",
                "childNodeId": "Workflow1:Sequence1",
                "methodName": "OnRootCompletedAsync"
            },
            {
                "$id": "10",
                "ownerInstanceId": "1186acc0a9dba008",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "methodName": "OnChildCompleted"
            },
            {
                "$id": "11",
                "ownerInstanceId": "4255e742bd28b043",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "methodName": "OnChildCompleted",
                "tag": "a4ca12e0-3142-49f9-9743-8de42699bcca"
            },
            {
                "$id": "12",
                "ownerInstanceId": "4255e742bd28b043",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "methodName": "OnChildCompleted",
                "tag": "4482e333-3f41-4d69-9fae-4acd65a521cb"
            },
            {
                "$id": "13",
                "ownerInstanceId": "483e756c2209dc50",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "methodName": "OnChildCompleted"
            },
            {
                "$id": "14",
                "ownerInstanceId": "bf65546951b8d641",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "methodName": "OnChildCompleted"
            }
        ]
    },
    "activityExecutionContexts": {
        "$id": "15",
        "$values": [
            {
                "$id": "16",
                "id": "f3ed169633664dbb",
                "parentContextId": "bf65546951b8d641",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "properties": {
                    "$id": "17"
                },
                "activityState": {
                    "$id": "18",
                    "TaskName": "PromptVote",
                    "Payload": {
                        "User": {
                            "id": "2",
                            "name": "User 2"
                        }
                    }
                },
                "dynamicVariables": {
                    "$id": "19",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2120282+00:00"
            },
            {
                "$id": "20",
                "id": "c2547d312d23f76",
                "parentContextId": "483e756c2209dc50",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "properties": {
                    "$id": "21"
                },
                "activityState": {
                    "$id": "22",
                    "TaskName": "PromptVote",
                    "Payload": {
                        "User": {
                            "id": "1",
                            "name": "User 1"
                        }
                    }
                },
                "dynamicVariables": {
                    "$id": "23",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2082886+00:00"
            },
            {
                "$id": "24",
                "id": "bf65546951b8d641",
                "parentContextId": "4255e742bd28b043",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "properties": {
                    "$id": "25",
                    "CurrentIndex": 1,
                    "PersistentVariablesDictionary": {
                        "e2392bdcd6db480795a3ff41f11aa718": {
                            "$id": "26",
                            "id": "2",
                            "name": "User 2",
                            "_type": "Elsa.Demo.API.Workflows.User, API"
                        },
                        "cbe8234ab2ca4e9a9e871bbce3a492ed": 1,
                        "_type": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "27"
                },
                "dynamicVariables": {
                    "$id": "28",
                    "$values": [
                        {
                            "$id": "29",
                            "id": "e2392bdcd6db480795a3ff41f11aa718",
                            "name": "CurrentValue",
                            "typeName": "Elsa.Demo.API.Workflows.User, API",
                            "value": "User { Id = 2, Name = User 2 }",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        },
                        {
                            "$id": "30",
                            "id": "cbe8234ab2ca4e9a9e871bbce3a492ed",
                            "name": "CurrentIndex",
                            "typeName": "Int32",
                            "value": "1",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        }
                    ]
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2082554+00:00",
                "tag": "4482e333-3f41-4d69-9fae-4acd65a521cb"
            },
            {
                "$id": "31",
                "id": "483e756c2209dc50",
                "parentContextId": "4255e742bd28b043",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "properties": {
                    "$id": "32",
                    "CurrentIndex": 1,
                    "PersistentVariablesDictionary": {
                        "b5edb20f3d3a4468946201eff4bffdcb": {
                            "$id": "33",
                            "id": "1",
                            "name": "User 1",
                            "_type": "Elsa.Demo.API.Workflows.User, API"
                        },
                        "09c7da15d48a4e38abb5e28cf13a5c7a": 0,
                        "_type": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "34"
                },
                "dynamicVariables": {
                    "$id": "35",
                    "$values": [
                        {
                            "$id": "36",
                            "id": "b5edb20f3d3a4468946201eff4bffdcb",
                            "name": "CurrentValue",
                            "typeName": "Elsa.Demo.API.Workflows.User, API",
                            "value": "User { Id = 1, Name = User 1 }",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        },
                        {
                            "$id": "37",
                            "id": "09c7da15d48a4e38abb5e28cf13a5c7a",
                            "name": "CurrentIndex",
                            "typeName": "Int32",
                            "value": "0",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        }
                    ]
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2081475+00:00",
                "tag": "a4ca12e0-3142-49f9-9743-8de42699bcca"
            },
            {
                "$id": "38",
                "id": "4255e742bd28b043",
                "parentContextId": "1186acc0a9dba008",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "ownerActivityNodeId": "Workflow1:Sequence1",
                "properties": {
                    "$id": "39",
                    "ScheduledTagsProperty": {
                        "$id": "40",
                        "$values": [
                            "a4ca12e0-3142-49f9-9743-8de42699bcca",
                            "4482e333-3f41-4d69-9fae-4acd65a521cb"
                        ],
                        "_type": "System.Collections.Generic.List\u00601[[System.Guid, System.Private.CoreLib]], System.Private.CoreLib"
                    },
                    "CompletedTagsProperty": {
                        "$id": "41",
                        "$values": [],
                        "_type": "System.Collections.Generic.List\u00601[[System.Guid, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "42",
                    "Items": [
                        {
                            "id": "1",
                            "name": "User 1"
                        },
                        {
                            "id": "2",
                            "name": "User 2"
                        }
                    ]
                },
                "dynamicVariables": {
                    "$id": "43",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2038384+00:00"
            },
            {
                "$id": "44",
                "id": "1186acc0a9dba008",
                "parentContextId": "184693755396caa",
                "scheduledActivityNodeId": "Workflow1:Sequence1",
                "ownerActivityNodeId": "Workflow1",
                "properties": {
                    "$id": "45",
                    "CurrentIndex": 3
                },
                "activityState": {
                    "$id": "46"
                },
                "dynamicVariables": {
                    "$id": "47",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.1951791+00:00"
            },
            {
                "$id": "48",
                "id": "184693755396caa",
                "scheduledActivityNodeId": "Workflow1",
                "properties": {
                    "$id": "49",
                    "PersistentVariablesDictionary": {
                        "Workflow1:variable-1": {
                            "$id": "50",
                            "members": {
                                "$id": "51",
                                "$values": [
                                    {
                                        "$ref": "33"
                                    },
                                    {
                                        "$ref": "26"
                                    }
                                ]
                            },
                            "_type": "Elsa.Demo.API.Workflows.VotingBoard, API"
                        },
                        "_type": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "52"
                },
                "dynamicVariables": {
                    "$id": "53",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.1950831+00:00"
            }
        ]
    },
    "scheduledActivities": {
        "$id": "54",
        "$values": []
    },
    "executionLogSequence": 13,
    "input": {
        "$id": "55"
    },
    "output": {
        "$id": "56"
    },
    "properties": {
        "$id": "57"
    },
    "createdAt": "2024-04-18T12:27:32.1942066+00:00",
    "updatedAt": "2024-04-18T12:27:32.213311+00:00"
}
```

And here is after we try to submit any task input:

```json
{
    "$id": "1",
    "id": "7d347caaac3cb6a8",
    "definitionId": "QuorumVotingWorkflow",
    "definitionVersionId": "cd7291eace9fe745",
    "definitionVersion": 1,
    "status": "Finished",
    "subStatus": "Faulted",
    "bookmarks": {
        "$id": "2",
        "$values": [
            {
                "$id": "3",
                "id": "f18cc5c0ab31ee7",
                "name": "Elsa.RunTask",
                "hash": "42BA89CC4020B206D2A788EF011E1A551DA69A4013234C2AAF824F8747E3046D",
                "payload": {
                    "$id": "4",
                    "taskId": "2b0778610e37f35a",
                    "taskName": "PromptVote",
                    "_type": "Elsa.Workflows.Runtime.Bookmarks.RunTaskBookmarkPayload, Elsa.Workflows.Runtime"
                },
                "activityId": "RunTask1",
                "activityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "activityInstanceId": "f3ed169633664dbb",
                "createdAt": "2024-04-18T12:27:32.2130232+00:00",
                "autoBurn": true,
                "callbackMethodName": "ResumeAsync",
                "autoComplete": true
            }
        ]
    },
    "incidents": {
        "$id": "5",
        "$values": [
            {
                "$id": "6",
                "activityId": "SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E1",
                "activityType": "Elsa.SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E",
                "message": "Object reference not set to an instance of an object.",
                "exception": {
                    "$id": "7",
                    "type": "System.NullReferenceException, System.Private.CoreLib",
                    "message": "Object reference not set to an instance of an object.",
                    "stackTrace": "   at Elsa.Demo.API.Workflows.WorkflowExtensions.\u003C\u003Ec__DisplayClass0_0.\u003CSetVotes\u003Eb__0(ExpressionExecutionContext context) in C:\\Users\\Oleksii_Nikiforov\\adgm\\workflow-engine\\src\\API\\Workflows\\WorkflowExtensions.cs:line 20\r\n   at Elsa.Expressions.Models.Expression.\u003C\u003Ec__DisplayClass14_0\u00601.\u003CDelegateExpression\u003Eb__0(ExpressionExecutionContext context)\r\n   at Elsa.Expressions.DelegateExpressionHandler.EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions options)\r\n   at Elsa.Expressions.Services.ExpressionEvaluator.EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions options)\r\n   at Elsa.Extensions.ActivityExecutionContextExtensions.EvaluateInputPropertyAsync(ActivityExecutionContext context, ActivityDescriptor activityDescriptor, InputDescriptor inputDescriptor)\r\n   at Elsa.Extensions.ActivityExecutionContextExtensions.EvaluateInputPropertiesAsync(ActivityExecutionContext context)\r\n   at Elsa.Workflows.Middleware.Activities.DefaultActivityInvokerMiddleware.EvaluateInputPropertiesAsync(ActivityExecutionContext context)\r\n   at Elsa.Workflows.Middleware.Activities.DefaultActivityInvokerMiddleware.InvokeAsync(ActivityExecutionContext context)\r\n   at Elsa.Workflows.Middleware.Activities.NotificationPublishingMiddleware.InvokeAsync(ActivityExecutionContext context)\r\n   at Elsa.Workflows.Middleware.Activities.ExecutionLogMiddleware.InvokeAsync(ActivityExecutionContext context)\r\n   at Elsa.Workflows.Middleware.Activities.ExceptionHandlingMiddleware.InvokeAsync(ActivityExecutionContext context)"
                },
                "timestamp": "2024-04-18T12:28:17.5624742+00:00"
            }
        ]
    },
    "isSystem": false,
    "completionCallbacks": {
        "$id": "8",
        "$values": [
            {
                "$id": "9",
                "ownerInstanceId": "184693755396caa",
                "childNodeId": "Workflow1:Sequence1",
                "methodName": "OnRootCompletedAsync"
            },
            {
                "$id": "10",
                "ownerInstanceId": "1186acc0a9dba008",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "methodName": "OnChildCompleted"
            },
            {
                "$id": "11",
                "ownerInstanceId": "4255e742bd28b043",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "methodName": "OnChildCompleted",
                "tag": "a4ca12e0-3142-49f9-9743-8de42699bcca"
            },
            {
                "$id": "12",
                "ownerInstanceId": "4255e742bd28b043",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "methodName": "OnChildCompleted",
                "tag": "4482e333-3f41-4d69-9fae-4acd65a521cb"
            },
            {
                "$id": "13",
                "ownerInstanceId": "bf65546951b8d641",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "methodName": "OnChildCompleted"
            },
            {
                "$id": "14",
                "ownerInstanceId": "483e756c2209dc50",
                "childNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E1",
                "methodName": "OnChildCompleted"
            }
        ]
    },
    "activityExecutionContexts": {
        "$id": "15",
        "$values": [
            {
                "$id": "16",
                "id": "395c72ff32d551bf",
                "parentContextId": "483e756c2209dc50",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:SetVariable\u003CDictionary\u003CString,Boolean\u003E\u003E1",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "properties": {
                    "$id": "17"
                },
                "activityState": {
                    "$id": "18",
                    "Variable": {
                        "id": "Workflow1:variable-3",
                        "name": "Variable_2",
                        "typeName": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Boolean, System.Private.CoreLib]], System.Private.CoreLib",
                        "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                    }
                },
                "dynamicVariables": {
                    "$id": "19",
                    "$values": []
                },
                "status": "Faulted",
                "startedAt": "2024-04-18T12:28:17.5452525+00:00"
            },
            {
                "$id": "20",
                "id": "184693755396caa",
                "scheduledActivityNodeId": "Workflow1",
                "properties": {
                    "$id": "21",
                    "PersistentVariablesDictionary": {
                        "Workflow1:variable-2": false,
                        "_type": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "22"
                },
                "dynamicVariables": {
                    "$id": "23",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.1950831+00:00"
            },
            {
                "$id": "24",
                "id": "1186acc0a9dba008",
                "parentContextId": "184693755396caa",
                "scheduledActivityNodeId": "Workflow1:Sequence1",
                "ownerActivityNodeId": "Workflow1",
                "properties": {
                    "$id": "25",
                    "CurrentIndex": 3
                },
                "activityState": {
                    "$id": "26"
                },
                "dynamicVariables": {
                    "$id": "27",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.1951791+00:00"
            },
            {
                "$id": "28",
                "id": "4255e742bd28b043",
                "parentContextId": "1186acc0a9dba008",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "ownerActivityNodeId": "Workflow1:Sequence1",
                "properties": {
                    "$id": "29",
                    "ScheduledTagsProperty": {
                        "$id": "30",
                        "$values": [
                            "a4ca12e0-3142-49f9-9743-8de42699bcca",
                            "4482e333-3f41-4d69-9fae-4acd65a521cb"
                        ],
                        "_type": "System.Collections.Generic.List\u00601[[System.Guid, System.Private.CoreLib]], System.Private.CoreLib"
                    },
                    "CompletedTagsProperty": {
                        "$id": "31",
                        "$values": [],
                        "_type": "System.Collections.Generic.List\u00601[[System.Guid, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "32",
                    "Items": {
                        "$id": "33",
                        "$values": [
                            {
                                "id": "1",
                                "name": "User 1"
                            },
                            {
                                "id": "2",
                                "name": "User 2"
                            }
                        ],
                        "_type": "System.Collections.Generic.List\u00601[[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "dynamicVariables": {
                    "$id": "34",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2038384+00:00"
            },
            {
                "$id": "35",
                "id": "483e756c2209dc50",
                "parentContextId": "4255e742bd28b043",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "properties": {
                    "$id": "36",
                    "CurrentIndex": 2,
                    "PersistentVariablesDictionary": {
                        "b5edb20f3d3a4468946201eff4bffdcb": {
                            "$id": "37",
                            "id": "1",
                            "name": "User 1",
                            "_type": "Elsa.Demo.API.Workflows.User, API"
                        },
                        "09c7da15d48a4e38abb5e28cf13a5c7a": 0,
                        "_type": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "38"
                },
                "dynamicVariables": {
                    "$id": "39",
                    "$values": [
                        {
                            "$id": "40",
                            "id": "b5edb20f3d3a4468946201eff4bffdcb",
                            "name": "CurrentValue",
                            "typeName": "Elsa.Demo.API.Workflows.User, API",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        },
                        {
                            "$id": "41",
                            "id": "09c7da15d48a4e38abb5e28cf13a5c7a",
                            "name": "CurrentIndex",
                            "typeName": "Int32",
                            "value": "0",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        }
                    ]
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2081475+00:00"
            },
            {
                "$id": "42",
                "id": "bf65546951b8d641",
                "parentContextId": "4255e742bd28b043",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1",
                "properties": {
                    "$id": "43",
                    "CurrentIndex": 1,
                    "PersistentVariablesDictionary": {
                        "e2392bdcd6db480795a3ff41f11aa718": {
                            "$id": "44",
                            "id": "2",
                            "name": "User 2",
                            "_type": "Elsa.Demo.API.Workflows.User, API"
                        },
                        "cbe8234ab2ca4e9a9e871bbce3a492ed": 1,
                        "_type": "System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Object, System.Private.CoreLib]], System.Private.CoreLib"
                    }
                },
                "activityState": {
                    "$id": "45"
                },
                "dynamicVariables": {
                    "$id": "46",
                    "$values": [
                        {
                            "$id": "47",
                            "id": "e2392bdcd6db480795a3ff41f11aa718",
                            "name": "CurrentValue",
                            "typeName": "Elsa.Demo.API.Workflows.User, API",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        },
                        {
                            "$id": "48",
                            "id": "cbe8234ab2ca4e9a9e871bbce3a492ed",
                            "name": "CurrentIndex",
                            "typeName": "Int32",
                            "value": "1",
                            "storageDriverTypeName": "Elsa.Workflows.Services.WorkflowStorageDriver, Elsa.Workflows.Core"
                        }
                    ]
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2082554+00:00",
                "tag": "4482e333-3f41-4d69-9fae-4acd65a521cb"
            },
            {
                "$id": "49",
                "id": "f3ed169633664dbb",
                "parentContextId": "bf65546951b8d641",
                "scheduledActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2:RunTask1",
                "ownerActivityNodeId": "Workflow1:Sequence1:ParallelForEach\u003CUser\u003E1:Sequence2",
                "properties": {
                    "$id": "50"
                },
                "activityState": {
                    "$id": "51",
                    "TaskName": "PromptVote",
                    "Payload": {
                        "User": {
                            "id": "2",
                            "name": "User 2"
                        }
                    }
                },
                "dynamicVariables": {
                    "$id": "52",
                    "$values": []
                },
                "status": "Running",
                "startedAt": "2024-04-18T12:27:32.2120282+00:00"
            }
        ]
    },
    "scheduledActivities": {
        "$id": "53",
        "$values": []
    },
    "executionLogSequence": 17,
    "input": {
        "$id": "54"
    },
    "output": {
        "$id": "55"
    },
    "properties": {
        "$id": "56"
    },
    "createdAt": "2024-04-18T12:27:32.1942066+00:00",
    "updatedAt": "2024-04-18T12:28:17.5632023+00:00",
    "finishedAt": "2024-04-18T12:28:17.5632023+00:00"
}
```

The issue is with `board.Get(context).Members` because the value of `board.Get(context)` is not restored and when we try hi `.Memebers` we get Null Reference Exception (NRE):

```csharp
new ParallelForEach<User>()
{
    // doesn't work - NRE
    Items = new(context => board.Get(context).Members),
    Body = new Sequence
    {
        Activities =
        {
            new RunTask("PromptVote")
            {
                Result = new Output<object>(currentVoteResult),
                Payload = new(context => new Dictionary<string, object>
                {
                    ["User"] = context.GetVariable<User>("CurrentValue"),
                })
            },
            new SetVariable<Dictionary<string, bool>>(
                votes,
                WorkflowExtensions.SetVotes(currentVoteResult, votes)
            )
        }
    },
},
```
