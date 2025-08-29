# Introduction

The Extra.Guard library provides a means for verifying preconditions on function
arguments. The methods exposed for doing so return the validated parameter
whenever it does not violate the precondition that is being tested, otherwise an
exception is thrown.

## Motivation

This library draws inspiration from the
[Java standard library](https://docs.oracle.com/javase/8/docs/api/java/util/Objects.html#requireNonNull-T-).
It also takes cues from similar
[libraries](https://github.com/ardalis/guardclauses) for the .NET platform. Like those libraries, this one intends
to abstract a repetitive task, and provide developers with a tool for reducing
boilerplate code. Any particularities of this library reflect the authors'
attitude towards programming in general, and argument validation specifically.