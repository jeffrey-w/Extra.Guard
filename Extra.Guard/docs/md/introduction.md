# Introduction

The Extra.Guard library provides a means for verifying preconditions on function
arguments. The methods exposed for doing so return the validated parameter
whenever it does not violate the precondition that is being tested, otherwise an
exception is thrown.

## Motivation

This library draws influence from methods provided by the
[Java standard library](https://docs.oracle.com/javase/8/docs/api/java/util/Objects.html#requireNonNull-T-).
It also takes many cues from similar
[libraries](https://github.com/ardalis/guardclauses) for the .NET platform. The
modifications and refinements made to implement this library reflect the author's
personal preferences and attitudes towards programming in general and argument
validation specifically.