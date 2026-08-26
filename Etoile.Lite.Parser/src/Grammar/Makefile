Generated/ArcaeaAffChartVisitor.cs: ArcaeaAffChart.g4
	java \
		-jar ../../tools/antlr-4.13.2-complete.jar \
		-Dlanguage=CSharp \
		-visitor \
		-package ArcCreate.ChartFormat.Grammar \
		-o Generated \
		ArcaeaAffChart.g4
		
	dos2unix Generated/*.cs

	echo "Remove unnecessary files" && \
		rm -f Generated/*.java && \
		rm -f Generated/*.tokens && \
		rm -f Generated/*.interp && \
		rm -f Generated/*.meta && \
		echo "Cleaned"