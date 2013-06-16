NAntFind
========

	<property name="find.module.path" value="find;dependency/more/find" />

    <find package="SQLServer" required="true" version="100" />
    <echo message="${SQLServer.found}" />
    <echo message="${SQLServer}" />

    <find file="SQLCmd.exe" package="SQLServer" version="100" recursive="true"/>
    <echo message="${SQLCmd.exe.found}" />
    <echo message="${SQLCmd.exe}" />

NAntFind is a CMake find style dependency discovery extension for NAnt.

The FindSQLServer.include

    <package name="SQLServer">
        <version value="foo">
        	<hints>
    			<hint value="C:\Program Files\Microsoft SQL Server\bar\Tools\Binn" />
    			<hint value="C:\Program Files (x86)\Microsoft SQL Server\bar\Tools\Binn" />
    		</hints>
    		<names>
    			<name value="SQLCmd.exe" />
    			<name value="sqlmonitor.exe" />
    		</names>
    	</version>
    	<version>
    		<hints>
    			<hint value="C:\Program Files\Microsoft SQL Server\${version}\Tools\Binn" />
    			<hint value="C:\Program Files (x86)\Microsoft SQL Server\${version}\Tools\Binn" />
    		</hints>
    		<names>
    			<name value="SQLCmd.exe" />
    			<name value="sqlmonitor.exe" />
    		</names>
    	</version>
    </package>

The file name must be _Find_ + <package name> + _.include_, so NAntFind can locate your find module automatically.

----
Feature
----
* Find packages.
* Search for find modules in ${find.module.path}
* Search for a specific version
* Default package version can be specified

Result can be queried in NAnt variables:
* **${package.found}**: True/False
* **${package}**: Package path
* **${package.version}**: Package version.

----
TODO
----
* Find for one file in a given package
* Remove dependency to NAnt, but can be used in NAnt (can benefit NAnt build-in functions)
* A more flexible and easy to use DSL

----
License
----
[GNU Public License][1]


  [1]: http://www.gnu.org/copyleft/gpl.html
