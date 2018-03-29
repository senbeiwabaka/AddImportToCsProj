@echo help!
cd {cdlocation}
@call msbuild.exe {solutionpath} /t:rebuild /p:RunCodeAnalysis=true /p:CodeAnalysisLogFile=code-analysis.xml /m:8 /ds /v:q