NAntFind
========

    <find package="SQLServer" required="true" version="100" />
    <echo message="${SQLServer.found}" /> 
    <echo message="${SQLServer}" />

    <find file="SQLCmd.exe" package="SQLServer" version="100" recursive="true"/>
    <echo message="${SQLCmd.exe.found}" />
    <echo message="${SQLCmd.exe}" />

NAntFind is a c CMake find style dependency discovery extension for NAnt.

The FindSQLServer.include

    <package name="SQLServer" default="100">
        <module version="100">
    		<hints>
    			<hint value="C:\Program Files\Microsoft SQL Server\100\Tools\Binn" />
    			<hint value="C:\Program Files (x86)\Microsoft SQL Server\100\Tools\Binn" />
    		</hints>
    		<names>
    			<name value="SQLCmd.exe" />
    			<name value="sqlmonitor.exe" />
    		</names>
    	</module>
    </package>

The file name must be _Find_ + <module name> + _.include_, so NAntFind can locate your find module automatically.

----
TODO
----
* Find for package
* Find for one file in a given package
* Search for find modules in ${find.module.path}
* Remove dependency to NAnt, but can be used in NAnt (can benefit NAnt build-in functions)
* Search for package of specific version
* Support default package version
* A more flexible and easy to use DSL

----
License
----
[GNU Public License][1]


  [1]: http://www.gnu.org/copyleft/gpl.html
