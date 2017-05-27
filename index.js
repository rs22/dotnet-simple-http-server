#!/usr/bin/env node
const spawn = require('child_process').spawn

const dotnet = spawn('dotnet', ['run', '-c', 'Release', process.cwd(), ...process.argv], { cwd: __dirname });

dotnet.stdout.pipe(process.stdout);
dotnet.stderr.pipe(process.stderr);
